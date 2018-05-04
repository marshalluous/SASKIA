using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.DeMorganSimplifier
{
    public sealed class DeMorganSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.DeMorganSimplifier.GetDiagnosticId();
        public string Title => RefactoringMessages.DeMorganSimplifierTitle();
        public string Description => RefactoringMessages.DeMorganSimplifierDescription();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var actualExpression = node.ToString();
            return GetFixableNodes(node) == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                CreateFailedDiagnosticResult(actualExpression);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var notNode = (PrefixUnaryExpressionSyntax)node;
            var binaryExpressionNode = GetBinaryExpression(notNode);
            return binaryExpressionNode == null ? null : ApplyDeMorgan(binaryExpressionNode);
        }
        
        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            GetReplaceableRootNode(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.LogicalNotExpression };

        public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<PrefixUnaryExpressionSyntax>(token);

        private static DiagnosticInfo CreateFailedDiagnosticResult(string actualExpression) =>
            DiagnosticInfo.CreateFailedResult(RefactoringMessages.DeMorganSimplifierMessage(actualExpression));

        private static SyntaxKind GetInvertedSyntaxKind(SyntaxKind syntaxKind) =>
            syntaxKind == SyntaxKind.BarBarToken ?
                SyntaxKind.LogicalAndExpression 
                : SyntaxKind.LogicalOrExpression;

        private static BinaryExpressionSyntax GetBinaryExpression(PrefixUnaryExpressionSyntax prefixNode)
        {
            if (!(prefixNode.Operand is ParenthesizedExpressionSyntax parenthesizedNode))
                return null;
            if (!(parenthesizedNode.Expression is BinaryExpressionSyntax binaryExpressionNode))
                return null;
            return binaryExpressionNode;
        }

        private static ExpressionSyntax Not(ExpressionSyntax node) =>
            SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxNodeHelper.AddParentheses(node));

        private static IEnumerable<SyntaxNode> CreateResultNode(SyntaxKind operatorKind, ExpressionSyntax leftHandSide,
            ExpressionSyntax rightHandSide) =>
            new[] { SyntaxFactory.BinaryExpression(operatorKind, leftHandSide, rightHandSide)
                .NormalizeWhitespace() };
        
        private static IEnumerable<SyntaxNode> ApplyDeMorgan(BinaryExpressionSyntax binaryExpressionNode)
        {
            var operatorKind = binaryExpressionNode.OperatorToken.Kind();

            if (operatorKind == SyntaxKind.BarBarToken || operatorKind == SyntaxKind.AmpersandAmpersandToken)
            {
                return CreateResultNode(GetInvertedSyntaxKind(operatorKind), Not(binaryExpressionNode.Left),
                    Not(binaryExpressionNode.Right));
            }

            return null;
        }
	}
}