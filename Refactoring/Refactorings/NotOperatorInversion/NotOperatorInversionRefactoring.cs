using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.NotOperatorInversion
{
    public sealed class NotOperatorInversionRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA911";

        public string Title => DiagnosticId;

        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.LogicalNotExpression};
            

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var c = ApplyFix(node);

            return c == null ? 
                DiagnosticInfo.CreateSuccessfulResult() : 
                DiagnosticInfo.CreateFailedResult("failed result");
        }

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            var logicalNotExpression = (PrefixUnaryExpressionSyntax) node;

            if (logicalNotExpression.OperatorToken.Kind() != SyntaxKind.ExclamationToken)
                return null;

            if (!(logicalNotExpression.Operand is ParenthesizedExpressionSyntax operandExpression))
                return null;

            var expression = operandExpression.Expression;

            if (!(expression is BinaryExpressionSyntax binaryExpression))
                return null;

            var notMap = new Dictionary<SyntaxKind, SyntaxKind>
            {
                [SyntaxKind.LessThanToken] = SyntaxKind.GreaterThanOrEqualExpression,
                [SyntaxKind.GreaterThanToken] = SyntaxKind.LessThanOrEqualExpression,
                [SyntaxKind.LessThanEqualsToken] = SyntaxKind.GreaterThanExpression,
                [SyntaxKind.GreaterThanEqualsToken] = SyntaxKind.LessThanExpression,
                [SyntaxKind.EqualsEqualsToken] = SyntaxKind.NotEqualsExpression,
                [SyntaxKind.ExclamationEqualsToken] = SyntaxKind.EqualsExpression
            };

            var kind = binaryExpression.OperatorToken.Kind();

            if (notMap.ContainsKey(kind))
            {
                return new[] { SyntaxFactory.BinaryExpression(notMap[kind], binaryExpression.Left, binaryExpression.Right) };
            }

            return null;
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<PrefixUnaryExpressionSyntax>(token);
    }
}
