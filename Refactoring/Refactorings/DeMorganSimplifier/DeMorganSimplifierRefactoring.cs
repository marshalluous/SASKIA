using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.DeMorganSimplifier
{
    public sealed class DeMorganSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.DeMorganSimplifier.GetDiagnosticId();

        public string Title => RefactoringMessageFactory.DeMorganSimplifierTitle();

        public string Description => RefactoringMessageFactory.DeMorganSimplifierDescription();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var actualExpression = node.ToString();
            var fixedNodes = GetFixableNodes(node);
            
            return fixedNodes == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory.DeMorganSimplifierMessage(actualExpression));
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var notNode = (PrefixUnaryExpressionSyntax) node;
            var binaryExpressionNode = GetBinaryExpression(notNode);

            if (binaryExpressionNode == null)
                return null;

            var notLeftNode = Not(binaryExpressionNode.Left);
            var notRightNode = Not(binaryExpressionNode.Right);

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (binaryExpressionNode.OperatorToken.Kind())
            {
                case SyntaxKind.BarBarToken:
                    return CreateResultNode(SyntaxKind.LogicalAndExpression, notLeftNode, notRightNode);
                case SyntaxKind.AmpersandAmpersandToken:
                    return CreateResultNode(SyntaxKind.LogicalOrExpression, notLeftNode, notRightNode);
                default:
                    return null;
            }
        }

        private static BinaryExpressionSyntax GetBinaryExpression(PrefixUnaryExpressionSyntax prefixNode)
        {
            if (!(prefixNode.Operand is ParenthesizedExpressionSyntax parenthesizedNode))
                return null;

            var expressionNode = parenthesizedNode.Expression;

            if (!(expressionNode is BinaryExpressionSyntax binaryExpressionNode))
                return null;

            return binaryExpressionNode;
        }

        private static ExpressionSyntax Not(ExpressionSyntax node) =>
            SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxNodeHelper.AddParentheses(node));

        private static IEnumerable<SyntaxNode> CreateResultNode(SyntaxKind operatorKind, ExpressionSyntax leftHandSide,
            ExpressionSyntax rightHandSide) =>
            new[] { SyntaxFactory.BinaryExpression(operatorKind, leftHandSide, rightHandSide)
                .NormalizeWhitespace() };

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            GetReplaceableRootNode(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.LogicalNotExpression };

		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<PrefixUnaryExpressionSyntax>(token);
	}
}
