using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.IntegerConstantSimplifier
{
    public sealed class IntegerConstantSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.IntegerConstantSimplifier.GetDiagnosticId();
        public string Title => RefactoringMessages.IntegerConstantSimplifierTitle();
        public string Description => RefactoringMessages.IntegerConstantSimplifierDescription();
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);
        
		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            SyntaxNodeHelper.GetExpressionSyntaxKinds();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var parentValue = EvaluateValue(node.Parent);

            if (node is LiteralExpressionSyntax || parentValue != null || IsUnaryMinusLiteral(node))
                return DiagnosticInfo.CreateSuccessfulResult();
            
            var value = EvaluateValue(node);
            return value == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult(RefactoringMessages.IntegerConstantSimplifierMessage(value.Value));
        }

        private static bool IsUnaryMinusLiteral(SyntaxNode node) => 
            node is PrefixUnaryExpressionSyntax prefixNode &&
            prefixNode.OperatorToken.Kind() == SyntaxKind.MinusToken &&
            node.ChildNodes().Count() == 1 &&
            node.ChildNodes().First() is LiteralExpressionSyntax;

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var value = EvaluateValue(node);

            if (value == null || node is LiteralExpressionSyntax)
                return null;

            var literal = SyntaxFactory.Literal(value.Value);
            return new [] { SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, literal) };
        }

        private static int? EvaluateValue(SyntaxNode node)
        {
            var visitor = new IntegerConstantSimplifierVisitor();
            return visitor.Visit(node);
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            var node = token.Parent;
            while (EvaluateValue(node.Parent) != null)
                node = node.Parent;
            return node;
        }
    }
}