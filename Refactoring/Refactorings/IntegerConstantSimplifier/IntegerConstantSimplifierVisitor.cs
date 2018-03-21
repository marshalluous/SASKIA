using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.IntegerConstantSimplifier
{
    internal sealed class IntegerConstantSimplifierVisitor : CSharpSyntaxVisitor<int?>
    {
        public override int? VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            var value = node.Token.Value;

            if (value is int intValue)
                return intValue;

            return null;
        }

        public override int? VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            return node.Expression.Accept(this);
        }

        public override int? VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var value = node.Operand?.Accept(this);

            if (value == null)
                return null;

            switch (node.OperatorToken.Kind())
            {
                case SyntaxKind.PlusToken:
                    return value;

                case SyntaxKind.MinusToken:
                    return -value;

                case SyntaxKind.TildeToken:
                    return ~value;

                default:
                    return null;
            }
        }

        public override int? VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var leftValue = node.Left?.Accept(this);
            var rightValue = node.Right?.Accept(this);

            if (leftValue == null || rightValue == null)
                return null;

            switch (node.OperatorToken.Kind())
            {
                case SyntaxKind.PlusToken:
                    return leftValue.Value + rightValue.Value;

                case SyntaxKind.MinusToken:
                    return leftValue.Value - rightValue.Value;

                case SyntaxKind.PercentToken:
                    return leftValue.Value % rightValue.Value;

                case SyntaxKind.SlashToken:
                    return leftValue.Value / rightValue.Value;

                case SyntaxKind.AsteriskToken:
                    return leftValue.Value * rightValue.Value;
                    
                default:
                    return null;
            }
        }
    }
}