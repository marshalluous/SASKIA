using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.BooleanConstantSimplifier
{
    public sealed class BooleanConstantSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.BooleanConstantSimplifier.GetDiagnosticId();
        public string Title => RefactoringMessageFactory.BooleanConstantSimplifierTitle();
        public string Description => RefactoringMessageFactory.BooleanConstantSimplifierDescription();

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            SyntaxNodeHelper.GetExpressionSyntaxKinds();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var value = EvaluateNode(node);
            if (value == null || IsNestedExpression(node) || node is LiteralExpressionSyntax)
                return DiagnosticInfo.CreateSuccessfulResult();
            return CreateFailedDiagnosticNode(value.Value);
        }

        private static DiagnosticInfo CreateFailedDiagnosticNode(bool value) =>
            DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory
                .BooleanConstantSimplifierMessage(value));

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var value = EvaluateNode(node);
            return value == null ? null : new [] { LiteralNode(value.Value) };
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorWithPredicate(token,
                node => !(IsBooleanExpression(node) && IsBooleanExpression(node.Parent)));
		
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

        private static bool? EvaluateNode(SyntaxNode node)
        {
            var visitor = new BooleanConstantSimplifierVisitor();
            return visitor.Visit(node);
        }

        private static SyntaxNode LiteralNode(bool value) =>
            SyntaxFactory.LiteralExpression(
                value ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);

		private static bool IsNestedExpression(SyntaxNode node) =>
		    node.Parent is PrefixUnaryExpressionSyntax ||
                node.Parent is BinaryExpressionSyntax ||
                node.Parent is ParenthesizedExpressionSyntax;
        
        private static bool IsBooleanExpression(SyntaxNode node) =>
            IsNotExpression(node) || IsOrAndExpression(node) || node is ParenthesizedExpressionSyntax ||
                   node is LiteralExpressionSyntax;
        
        private static bool IsNotExpression(SyntaxNode node) =>
            node is PrefixUnaryExpressionSyntax prefixNode &&
                   prefixNode.OperatorToken.Kind() == SyntaxKind.ExclamationToken;
        
        private static bool IsOrAndExpression(SyntaxNode node) =>
            node is BinaryExpressionSyntax binaryNode &&
                (binaryNode.OperatorToken.Kind() == SyntaxKind.AmpersandAmpersandToken ||
                binaryNode.OperatorToken.Kind() == SyntaxKind.BarBarToken);
    }
}