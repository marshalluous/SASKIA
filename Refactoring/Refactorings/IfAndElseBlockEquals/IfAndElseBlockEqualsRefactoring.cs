using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.IfAndElseBlockEquals
{
    public sealed class IfAndElseBlockEqualsRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA500";
        public string Title => "If and else block are the same";
        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.IfStatement };

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax)node;
            var thenBlock = ifNode.Statement;

            if (ifNode.Else == null)
                return DiagnosticInfo.CreateSuccessfulResult();

            var elseBlock = ifNode.Else.ChildNodes().First();

            return !ContainsElseIfStatement(ifNode) && CompareSyntaxNodes(thenBlock, elseBlock) ?
                DiagnosticInfo.CreateFailedResult("Then block and else block contains the same code") :
                DiagnosticInfo.CreateSuccessfulResult();
        }

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax) node;
            var thenBlock = ifNode.Statement;

            if (ifNode.Else == null)
                return new[] {node};

            var elseBlock = ifNode.Else.ChildNodes().First();

            return CompareSyntaxNodes(thenBlock, elseBlock) ? 
                ifNode.Statement.ChildNodes() : 
                new[] { node };
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<IfStatementSyntax>(token);

        private static bool ContainsElseIfStatement(IfStatementSyntax ifNode)
        {
            return ifNode.Else?.ChildNodes().First() is IfStatementSyntax;
        }

        private static bool CompareSyntaxNodes(SyntaxNode first, SyntaxNode second)
        {
            if (first is BlockSyntax firstBlock && firstBlock.ChildNodes().Count() == 1)
            {
                first = firstBlock.ChildNodes().First();
            }

            if (second is BlockSyntax secondBlock && secondBlock.ChildNodes().Count() == 1)
            {
                second = secondBlock.ChildNodes().First();
            }

            return first.GetType() == second.GetType() &&
                first.NormalizeWhitespace().GetText().ContentEquals(second.NormalizeWhitespace().GetText()) &&            
                CompareSyntaxNodeList(first.ChildNodes().ToList(), second.ChildNodes().ToList());
        }

        private static bool CompareSyntaxNodeList(IList<SyntaxNode> first, IList<SyntaxNode> second)
        {
            if (first.Count != second.Count)
                return false;

            var equalTrees = true;

            for (var index = 0; index < first.Count; ++index)
            {
                equalTrees &= CompareSyntaxNodes(first[index], second[index]);
            }

            return equalTrees;
        }
    }
}