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
        public string DiagnosticId => RefactoringId.IfAndElseBlockEquals.GetDiagnosticId();

        public string Title => RefactoringMessageFactory.IfAndElseBlockEqualsTitle();

        public string Description => RefactoringMessageFactory.IfAndElseBlockEqualsDescription();

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.IfStatement };

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax)node;

            return GetFixableNodes(node) == null
                ? DiagnosticInfo.CreateSuccessfulResult()
                : DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory.IfAndElseBlockEqualsMessage(),
                    markableLocation: ifNode.IfKeyword.GetLocation());
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax)node;
            
            if (ifNode.Else == null || ContainsElseIfStatement(ifNode))
                return null;

            var thenBlock = ifNode.Statement;
            var elseBlock = ifNode.Else.ChildNodes().First();

            var equalsValueClause = SyntaxFactory.EqualsValueClause(ifNode.Condition);

            var isPure = CheckExpressionPureness(node, ifNode);
            var localDeclarationStatement = CreateDeclarationStatement(equalsValueClause);

            if (!CompareSyntaxNodes(thenBlock, elseBlock))
                return null;

            if (!isPure)
                return new[] { localDeclarationStatement
                    .NormalizeWhitespace()
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
                }.Union(ifNode.Statement.ChildNodes());

            return ifNode.Statement.ChildNodes();
        }

        private static LocalDeclarationStatementSyntax CreateDeclarationStatement(EqualsValueClauseSyntax equalsValueClause)
        {
            var boolType = SyntaxFactory.ParseTypeName("bool");
            var localDeclarationStatement = SyntaxFactory
                .LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(boolType,
                    SyntaxFactory.SeparatedList(
                        new[] { SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("condition"), null, equalsValueClause) }
                        )).NormalizeWhitespace());
            return localDeclarationStatement;
        }

        private static bool CheckExpressionPureness(SyntaxNode node, IfStatementSyntax ifNode)
        {
            var compilationUnit = SyntaxNodeHelper.FindAncestorOfType<CompilationUnitSyntax>(node.GetFirstToken());
            var visitor = new PureExpressionCheckerVisitor(compilationUnit);
            return visitor.Visit(ifNode.Condition);
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<IfStatementSyntax>(token);

        private static bool ContainsElseIfStatement(IfStatementSyntax ifNode)
        {
            return ifNode.Else?.ChildNodes().First() is IfStatementSyntax;
        }

        private static bool CompareSyntaxNodes(SyntaxNode first, SyntaxNode second)
        {
            first = NormalizeIfBlock(first);
            second = NormalizeIfBlock(second);
            
            return first.GetType() == second.GetType() &&
                first.NormalizeWhitespace().GetText().ContentEquals(second.NormalizeWhitespace().GetText()) &&            
                CompareSyntaxNodeList(first.ChildNodes().ToList(), second.ChildNodes().ToList());
        }

        private static SyntaxNode NormalizeIfBlock(SyntaxNode block)
        {
            if (block is BlockSyntax && block.ChildNodes().Count() == 1)
                return block.ChildNodes().First();
            return block;
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