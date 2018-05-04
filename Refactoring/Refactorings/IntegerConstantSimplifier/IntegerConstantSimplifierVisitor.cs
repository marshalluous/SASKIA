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

        public override int? VisitParenthesizedExpression(ParenthesizedExpressionSyntax node) => 
            node.Expression.Accept(this);

        public override int? VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var value = node.Operand?.Accept(this);
            return value == null ? 
                null : 
                IntegerConstantOperatorSimplifier.ReducePrefixUnaryOperation(node.OperatorToken.Kind(), value.Value);
        }

        public override int? VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node) => null;

        public override int? VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var leftValue = node.Left?.Accept(this);
            var rightValue = node.Right?.Accept(this);

            if (leftValue == null || rightValue == null)
                return null;

            return IntegerConstantOperatorSimplifier.ReduceBinaryOperation(node.OperatorToken.Kind(), leftValue.Value,
                rightValue.Value);
        }
    }
}