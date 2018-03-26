using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.NotOperatorInversion
{
    internal static class ExpressionNotInverter
    {
        public static BinaryExpressionSyntax InvertBinaryExpression(BinaryExpressionSyntax binaryExpression)
        {
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
                return SyntaxFactory.BinaryExpression(notMap[kind], binaryExpression.Left, binaryExpression.Right)
                    .NormalizeWhitespace();
            }

            return null;
        }
    }
}
