using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.BooleanConstantSimplifier
{
    public sealed class BooleanConstantSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.BooleanConstantSimplifier.GetDiagnosticId();

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            SyntaxNodeHelper.GetExpressionSyntaxKinds();

        public string Title => RefactoringMessageFactory.BooleanConstantSimplifierTitle();

        public string Description => RefactoringMessageFactory.BooleanConstantSimplifierDescription();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var booleanVisitor = new BooleanConstantSimplifierVisitor();
            var value = booleanVisitor.Visit(node);

            if (IsNestedExpression(node))
                return DiagnosticInfo.CreateSuccessfulResult();

            return value != null && !(node is LiteralExpressionSyntax)
                ? DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory.BooleanConstantSimplifierMessage(value.Value))
                : DiagnosticInfo.CreateSuccessfulResult();
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var booleanVisitor = new BooleanConstantSimplifierVisitor();
            var value = booleanVisitor.Visit(node);

            if (value == null)
                yield return node;
            else
                yield return SyntaxFactory.LiteralExpression(value.Value
                    ? SyntaxKind.TrueLiteralExpression
                    : SyntaxKind.FalseLiteralExpression);
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorWithPredicate(token,
                node => !(IsBooleanExpression(node) && IsBooleanExpression(node.Parent)));
        
        private static bool IsNestedExpression(SyntaxNode node)
        {
            var parent = node.Parent;

            return parent is PrefixUnaryExpressionSyntax ||
                parent is BinaryExpressionSyntax ||
                parent is ParenthesizedExpressionSyntax;
        }

        private static bool IsBooleanExpression(SyntaxNode node)
        {
            switch (node)
            {
                case PrefixUnaryExpressionSyntax prefixNode:
                    return prefixNode.OperatorToken.Text == "!";

                case BinaryExpressionSyntax binaryNode:
                    return binaryNode.OperatorToken.Kind() == SyntaxKind.AmpersandAmpersandToken ||
                           binaryNode.OperatorToken.Kind() == SyntaxKind.BarBarToken;
            }

            return node is ParenthesizedExpressionSyntax || node is LiteralExpressionSyntax;
        }
    }
}