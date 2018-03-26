using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.BooleanConstantComparison
{
    public sealed class BooleanConstantComparisonRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA001";

        public string Title => "Comparison with boolean constant";

        public string Description => Title;

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var replaceNodes = new List<SyntaxNode>();
            InternApplyFix(node, replaceNodes);
            yield return replaceNodes.First();
        }

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var equalsEqualsNode = (BinaryExpressionSyntax) node;

            var diagnosticInfo = CheckForBooleanLiteral(equalsEqualsNode.Left);

            if (diagnosticInfo != null)
                return diagnosticInfo;

            return CheckForBooleanLiteral(equalsEqualsNode.Right) ??
                   DiagnosticInfo.CreateSuccessfulResult();
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return SyntaxNodeHelper.FindAncestorWithPredicate(token, node =>
            {
                if (node is BinaryExpressionSyntax binaryExpression)
                    return binaryExpression.OperatorToken.Text == "==" ||
                           binaryExpression.OperatorToken.Text == "!=";

                return false;
            });
        }

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize()
        {
            return new[] {SyntaxKind.EqualsExpression, SyntaxKind.NotEqualsExpression};
        }

        private static void InternApplyFix(SyntaxNode syntaxNode, ICollection<SyntaxNode> replaceNodes)
        {
            var binaryExpressionNode = (BinaryExpressionSyntax) syntaxNode;
            var compareOperator = binaryExpressionNode.OperatorToken.Text.Trim();
            ApplyRefactoring(binaryExpressionNode.Left, binaryExpressionNode.Right, replaceNodes, compareOperator);
            ApplyRefactoring(binaryExpressionNode.Right, binaryExpressionNode.Left, replaceNodes, compareOperator);
        }

        private static void ApplyRefactoring(SyntaxNode literalNode, ExpressionSyntax otherNode,
            ICollection<SyntaxNode> replaceNodes, string compareOperator)
        {
            if (!IsBooleanLiteralNode(literalNode, out var _))
                return;

            var not = IsTrueNode(literalNode) != (compareOperator == "==");

            replaceNodes.Add(not
                ? SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, 
                    SyntaxNodeHelper.AddParentheses(otherNode))
                : otherNode);
        }

        private static DiagnosticInfo CheckForBooleanLiteral(SyntaxNode syntaxNode)
        {
            return IsBooleanLiteralNode(syntaxNode, out var literalText)
                ? DiagnosticInfo.CreateFailedResult($"Unnecessary comparison with {literalText} literal detected")
                : null;
        }

        private static bool IsBooleanLiteralNode(SyntaxNode syntaxNode, out string literalText)
        {
            literalText = syntaxNode.GetText().ToString().Trim();
            return IsTrueNode(syntaxNode) || IsFalseNode(syntaxNode);
        }

        private static bool IsFalseNode(SyntaxNode syntaxNode)
        {
            return IsLiteralNode(syntaxNode, "false");
        }

        private static bool IsTrueNode(SyntaxNode syntaxNode)
        {
            return IsLiteralNode(syntaxNode, "true");
        }

        private static bool IsLiteralNode(SyntaxNode syntaxNode, string expectedLiteral)
        {
            return SyntaxNodeHelper.CheckNodeTypeAndText(syntaxNode, typeof(LiteralExpressionSyntax), expectedLiteral);
        }
    }
}