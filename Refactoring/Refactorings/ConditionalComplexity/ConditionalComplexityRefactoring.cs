using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.ConditionalComplexity
{
    public sealed class ConditionalComplexityRefactoring : IRefactoring
    {
        private const int ConditionalComplexityThreshold = 10;

        public string DiagnosticId => "SASKIA200";
        public string Title => DiagnosticId;
        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.MethodDeclaration};    

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax) node;
            var visitor = new ConditionalComplexityVisitor();
            var complexity = visitor.Visit(methodNode);
            
             return complexity > ConditionalComplexityThreshold ?
                DiagnosticInfo.CreateFailedResult($"Method is too complex (McCabe = {complexity})", complexity,
                    methodNode.Identifier.GetLocation()) :
                DiagnosticInfo.CreateSuccessfulResult(complexity);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            yield return node;
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return token.Parent;
        }
    }
}
