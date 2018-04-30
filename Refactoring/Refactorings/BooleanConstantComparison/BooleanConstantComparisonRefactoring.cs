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
        public string DiagnosticId => RefactoringId.BooleanConstantComparison.GetDiagnosticId();

        public string Title => RefactoringMessageFactory.BooleanConstantComparisonTitle();

        public string Description => RefactoringMessageFactory.BooleanConstantComparisonDescription();

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

		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorWithPredicate(token, IsEqualsComparisonNode);
    
        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.EqualsExpression, SyntaxKind.NotEqualsExpression};

        private static bool IsEqualsComparisonNode(SyntaxNode node)
        {
            if (node is BinaryExpressionSyntax binaryExpression)
                return binaryExpression.OperatorToken.Text == "==" ||
                       binaryExpression.OperatorToken.Text == "!=";

            return false;
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
                ? NegateNode(otherNode)
                : otherNode);
        }

        private static PrefixUnaryExpressionSyntax NegateNode(ExpressionSyntax otherNode) =>
            SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression,
                                SyntaxNodeHelper.AddParentheses(otherNode));
        
        private static DiagnosticInfo CheckForBooleanLiteral(SyntaxNode syntaxNode) =>
            IsBooleanLiteralNode(syntaxNode, out var literalText)
                ? DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory.BooleanConstantComparisonMessage(literalText == "true"))
                : null;
        
        private static bool IsBooleanLiteralNode(SyntaxNode syntaxNode, out string literalText)
        {
            literalText = SyntaxNodeHelper.GetText(syntaxNode);
            return IsTrueNode(syntaxNode) || IsFalseNode(syntaxNode);
        }

        private static bool IsFalseNode(SyntaxNode syntaxNode) =>
            IsLiteralNode(syntaxNode, "false");

        private static bool IsTrueNode(SyntaxNode syntaxNode) =>
            IsLiteralNode(syntaxNode, "true");

        private static bool IsLiteralNode(SyntaxNode syntaxNode, string expectedLiteral) =>
            SyntaxNodeHelper.CheckNodeTypeAndText(syntaxNode, typeof(LiteralExpressionSyntax), expectedLiteral);
    }
}