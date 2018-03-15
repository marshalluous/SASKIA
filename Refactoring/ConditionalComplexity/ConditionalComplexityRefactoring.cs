using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.ConditionalComplexity
{
    public sealed class ConditionalComplexityRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA200";
        public string Title => DiagnosticId;
        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.MethodDeclaration};    

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax) node;
            var visitor = new ConditionalComplexityVisitor();
            var complexity = visitor.VisitMethodDeclaration(methodNode);

            return complexity > 10 ?
                DiagnosticInfo.CreateFailedResult($"Method is too complex (McCabe = {complexity})") :
                DiagnosticInfo.CreateSuccessfulResult();
        }

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            yield return node;
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return token.Parent;
        }
    }
}
