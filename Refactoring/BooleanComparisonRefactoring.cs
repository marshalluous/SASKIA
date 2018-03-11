using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Simplification;

namespace Refactoring
{
    public sealed class BooleanComparisonRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA001";

        public string Title => "Comparison with boolean constant";

        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.NotEqualsExpression, SyntaxKind.EqualsExpression };

        DiagnosticInfo IRefactoring.DoDiagnosis(SyntaxNode node)
        {
            var equalsEqualsNode = (BinaryExpressionSyntax) node;

            if (IsBooleanLiteralNode(equalsEqualsNode.Left) || IsBooleanLiteralNode(equalsEqualsNode.Right))
            {
                return DiagnosticInfo.CreateFailedResult("Comparison with boolean constant");
            }

            return DiagnosticInfo.CreateSuccessfulResult();
        }
        
        public SyntaxNode GetReplaceableNode(SyntaxToken token)
            => GetParentBinaryExpressionNode(token.Parent.Parent);
        
        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            var result = new List<SyntaxNode>();
            InternApplyFix(node, result);
            return result;
        }
        
        private static bool IsBooleanLiteralNode(SyntaxNode expressionSyntax) =>
            IsTrueNode(expressionSyntax) || CheckNodeForBooleanLiteral(expressionSyntax, "false");

        private static bool IsTrueNode(SyntaxNode expressionSyntax) =>
            CheckNodeForBooleanLiteral(expressionSyntax, "true");

        private static bool CheckNodeForBooleanLiteral(SyntaxNode expressionSyntax, string expectedLiteralText)
        {
            var literalText = expressionSyntax.GetText().ToString().Trim();
            return expressionSyntax is LiteralExpressionSyntax && literalText == expectedLiteralText;
        }

        private static void ApplyRefactoringToOneOperatorSide(SyntaxNode checkLiteralNode, ExpressionSyntax otherNode,
            bool isEqualsOperator, ICollection<SyntaxNode> result)
        {
            if (IsBooleanLiteralNode(checkLiteralNode))
            {
                result.Add(IsTrueNode(checkLiteralNode) == isEqualsOperator
                    ? otherNode
                    : SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, AddParentheses(otherNode)));
            }
        }
        
        private static BinaryExpressionSyntax GetParentBinaryExpressionNode(SyntaxNode syntaxNode)
        {
            while (!(syntaxNode is BinaryExpressionSyntax))
            {
                syntaxNode = syntaxNode.Parent;
            }

            return (BinaryExpressionSyntax)syntaxNode;
        }

        private static void InternApplyFix(SyntaxNode node, ICollection<SyntaxNode> replaceNodes)
        {
            var operatorNode = (BinaryExpressionSyntax) node;
            var isEqualsOperator = operatorNode.OperatorToken.Text.Trim() == "==";
            ApplyRefactoringToOneOperatorSide(operatorNode.Left, operatorNode.Right, isEqualsOperator, replaceNodes);
            ApplyRefactoringToOneOperatorSide(operatorNode.Right, operatorNode.Left, isEqualsOperator, replaceNodes);
        }

        private static ExpressionSyntax AddParentheses(ExpressionSyntax expression)
        {
            if (expression is BinaryExpressionSyntax)
                return SyntaxFactory
                .ParenthesizedExpression(expression.NormalizeWhitespace())
                .WithAdditionalAnnotations(Simplifier.Annotation);

            return expression;
        }
    }
}