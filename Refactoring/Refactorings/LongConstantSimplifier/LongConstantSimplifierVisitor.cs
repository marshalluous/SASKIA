using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.LongConstantSimplifier
{
    internal sealed class LongConstantSimplifierVisitor : CSharpSyntaxVisitor<long?>
    {
        public override long? VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            var value = node.Token.Value;

            switch (value)
            {
                case int intValue:
                    return intValue;
                case long longValue:
                    return longValue;
            }

            return null;
        }

        public override long? VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            return node.Expression.Accept(this);
        }

        public override long? VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var value = node.Operand?.Accept(this);

            return value == null ? 
                null : 
                LongConstantOperatorSimplifier.ReducePrefixUnaryOperation(node.OperatorToken.Kind(), value.Value);
        }

        public override long? VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            return null;
        }

        public override long? VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var leftValue = node.Left?.Accept(this);
            var rightValue = node.Right?.Accept(this);

            if (leftValue == null || rightValue == null)
                return null;

            return LongConstantOperatorSimplifier.ReduceBinaryOperation(node.OperatorToken.Kind(), leftValue.Value,
                rightValue.Value);
        }
    }
}