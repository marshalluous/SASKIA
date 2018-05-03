using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.IfAndElseBlockEquals
{
    public sealed class IfAndElseBlockEqualsRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.IfAndElseBlockEquals.GetDiagnosticId();
        public string Title => RefactoringMessageFactory.IfAndElseBlockEqualsTitle();
        public string Description => RefactoringMessageFactory.IfAndElseBlockEqualsDescription();

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.IfStatement };
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);
        
		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax) node;
            return GetFixableNodes(node) == null
                ? DiagnosticInfo.CreateSuccessfulResult()
                : CreateFailedDiagnosticResult(ifNode);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax) node;

            if (!IfAndElseBlockEquals(ifNode))
                return null;

            var equalsValueClause = SyntaxFactory.EqualsValueClause(ifNode.Condition);
            var localDeclarationStatement = CreateDeclarationStatement(equalsValueClause);

            return !CheckExpressionPureness(node, ifNode) ? 
                ConditionStatement(localDeclarationStatement).Union(ifNode.Statement.ChildNodes()) :
                ifNode.Statement.ChildNodes();
        }

        private static bool IfAndElseBlockEquals(IfStatementSyntax ifNode)
        {
            if (ifNode.Else == null || ContainsElseIfStatement(ifNode))
                return false;

            var thenBlock = ifNode.Statement;
            var elseBlock = ifNode.Else.ChildNodes().First();
            return CompareSyntaxNodes(thenBlock, elseBlock);
        }

        private static IEnumerable<LocalDeclarationStatementSyntax> ConditionStatement(LocalDeclarationStatementSyntax localDeclarationStatement)
        {
            return new[] { localDeclarationStatement
                .NormalizeWhitespace()
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
            };
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

        private static bool ContainsElseIfStatement(IfStatementSyntax ifNode) =>
            ifNode.Else?.ChildNodes().First() is IfStatementSyntax;

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

        private static bool CompareSyntaxNodeList(ICollection<SyntaxNode> first, IList<SyntaxNode> second)
        {
            if (first.Count != second.Count)
                return false;

            return
                first.Count == second.Count &&
                    first.Select((node, index) => new {node, index})
                        .All(node => CompareSyntaxNodes(node.node, second[node.index]));
        }
        
        private static DiagnosticInfo CreateFailedDiagnosticResult(IfStatementSyntax ifNode) => 
            DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory.IfAndElseBlockEqualsMessage(),
                markableLocation: ifNode.IfKeyword.GetLocation());
    }
}