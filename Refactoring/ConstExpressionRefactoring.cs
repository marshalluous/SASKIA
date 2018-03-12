using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring
{
    public sealed class ConstExpressionRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA003";

        public string Title => "Constant expression detected";

        public string Description => Title;
        
        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            var value = VisitExpressionSyntaxNodes(node).Item2;
            return new[] { CreateLiteralNode(value) };   
        }

        private static SyntaxNode CreateLiteralNode(object value)
        {
            var type = typeof(SyntaxFactory);

            if (value is bool booleanValue)
            {
                return SyntaxFactory.LiteralExpression(booleanValue ?
                    SyntaxKind.TrueLiteralExpression :
                    SyntaxKind.FalseLiteralExpression);
            }

            var method = type.GetMethod("Literal", new[] { value.GetType() });
            var token = (SyntaxToken) method.Invoke(null, new[] { value });
            return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, token);
        }

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            return VisitExpressionSyntaxNodes(node).Item1;
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            var node = token.Parent;

            while (node is ExpressionSyntax && node.Parent is ExpressionSyntax)
            {
                node = node.Parent;
            }

            return node;
        }
        
        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            typeof(SyntaxKind)
                .GetEnumNames()
                .Where(name => name.EndsWith("Expression"))
                .Select(name => (SyntaxKind)Enum.Parse(typeof(SyntaxKind), name));

        private static Tuple<DiagnosticInfo, object> VisitExpressionSyntaxNodes(SyntaxNode node)
        {
            if (node is ExpressionSyntax expression)
            {
                var previousExpression = expression.GetText().ToString();
                var simplifiedExpression = ExpressionEvaluator.Evaluate(previousExpression);

                if (simplifiedExpression != null && previousExpression != simplifiedExpression.ToString())
                {
                    return Tuple.Create(DiagnosticInfo.CreateFailedResult("const expr found"), simplifiedExpression);
                }
            }
            
            foreach (var childNode in node.ChildNodes())
            {
                var r = VisitExpressionSyntaxNodes(childNode);

                if (r.Item1.DiagnosticFound)
                {
                    return r;
                }
            }

            return Tuple.Create(DiagnosticInfo.CreateSuccessfulResult(), (object) null);
        }
        
    }
}