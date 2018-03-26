using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.BooleanConstantSimplifier
{
    internal sealed class BooleanConstantSimplifierVisitor : CSharpSyntaxVisitor<bool?>
    {
        public override bool? VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            var value = node.Token.Value;

            if (value is bool boolValue)
                return boolValue;

            return null;
        }

        public override bool? VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            if (node.OperatorToken.Text == "!")
                return !node.Operand.Accept(this);

            return null;
        }

        public override bool? VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            return node.Expression.Accept(this);
        }

        public override bool? VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var leftValue = node.Left.Accept(this);
            var rightValue = node.Right.Accept(this);

            if (leftValue == null || rightValue == null)
                return null;

            switch (node.OperatorToken.Kind())
            {
                case SyntaxKind.AmpersandAmpersandToken:
                    return leftValue.Value && rightValue.Value;
                case SyntaxKind.BarBarToken:
                    return leftValue.Value || rightValue.Value;
                default:
                    return null;
            }
        }
    }
}