using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.IntegerConstantSimplifier
{
    public sealed class IntegerConstantSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA700";

        public string Title => DiagnosticId;

        public string Description => Title;

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            var visitor = new IntegerConstantSimplifierVisitor();
            var value = visitor.Visit(node);

            if (value == null)
                yield return node;
            else
            {
                var literal = SyntaxFactory.Literal(value.Value);
                yield return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, literal);
            }
        }

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var visitor = new IntegerConstantSimplifierVisitor();
            var value = visitor.Visit(node);

            return value == null && !(node is LiteralExpressionSyntax) ? 
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult("Integer constant can be simplified");
        }
        
        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            typeof(SyntaxKind)
                .GetEnumNames()
                .Where(name => name.EndsWith("Expression"))
                .Select(name => (SyntaxKind)Enum.Parse(typeof(SyntaxKind), name));


        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            var node = token.Parent;

            while (IsIntegerNode(node) && IsIntegerNode(node.Parent))
                node = node.Parent;

            return node;
        }

        private static bool IsIntegerNode(SyntaxNode node)
        {
            return node is BinaryExpressionSyntax ||
                   node is ParenthesizedExpressionSyntax ||
                   node is PrefixUnaryExpressionSyntax ||
                   node is LiteralExpressionSyntax;
        }
    }
}