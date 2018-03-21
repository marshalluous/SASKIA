using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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

            if (value == null)
                return DiagnosticInfo.CreateSuccessfulResult();
            else
                return DiagnosticInfo.CreateFailedResult("hallo");
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return null;
        }

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize()
        {
            yield return SyntaxKind.ClassDeclaration;
        }
    }
}
