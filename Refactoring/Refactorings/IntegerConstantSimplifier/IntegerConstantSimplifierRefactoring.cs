using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.IntegerConstantSimplifier
{
    public sealed class IntegerConstantSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA700";

        public string Title => DiagnosticId;

        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            SyntaxNodeHelper.GetExpressionSyntaxKinds();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var result = GetFixableNodes(node);
            return result == null && !(node is LiteralExpressionSyntax) ? 
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult("Integer constant can be simplified");
        }
        
        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var visitor = new IntegerConstantSimplifierVisitor();
            var value = visitor.Visit(node);

            if (value == null)
                return null;

            var literal = SyntaxFactory.Literal(value.Value);
            return new [] { SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, literal) };
        }
        
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