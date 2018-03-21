using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper
{
    internal sealed class PureExpressionCheckerVisitor : CSharpSyntaxVisitor<bool>
    {
        public override bool Visit(SyntaxNode node)
        {
            if (node is LiteralExpressionSyntax literalNode)
            {
                return true;
            }

            if (node is ParenthesizedExpressionSyntax parenthesizedNode)
            {
                return Visit(parenthesizedNode.Expression);
            }

            if (node is BinaryExpressionSyntax binaryExpression)
            {
                
            }

            if (node is PrefixUnaryExpressionSyntax prefixUnaryExpression)
            {

            }

            if (node is PostfixUnaryExpressionSyntax postfixUnaryExpression)
            {

            }

            return false;
        }
    }
}
