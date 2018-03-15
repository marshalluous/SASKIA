using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.BooleanConstantSimplifier
{
    public sealed class BooleanConstantSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA010";

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize()
        {
            return typeof(SyntaxKind)
                .GetEnumNames()
                .Where(name => name.EndsWith("Expression"))
                .Select(name => (SyntaxKind) Enum.Parse(typeof(SyntaxKind), name));
        }

        public string Title => "YXYX";

        public string Description => Title;

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var booleanVisitor = new BooleanConstantSimplifierVisitor();
            var value = booleanVisitor.Visit(node);

            return value != null
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

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            var node = token.Parent;

            while (node is ExpressionSyntax && node.Parent is ExpressionSyntax) node = node.Parent;

            return node;
        }
    }
}