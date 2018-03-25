using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.BooleanConstantSimplifier
{
    public sealed class BooleanConstantSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA010";

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            SyntaxNodeHelper.GetExpressionSyntaxKinds();

        public string Title => "YXYX";

        public string Description => Title;

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var booleanVisitor = new BooleanConstantSimplifierVisitor();
            var value = booleanVisitor.Visit(node);

            return value != null && !(node is LiteralExpressionSyntax)
                ? DiagnosticInfo.CreateFailedResult("Constant boolean expression can be simplified")
                : DiagnosticInfo.CreateSuccessfulResult();
        }

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
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