using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.ConditionalComplexity
{
    public sealed class ConditionalComplexityRefactoring : IRefactoring
    {
        private const int ConditionalComplexityThreshold = 10;

        public string DiagnosticId => RefactoringId.ConditionalComplexity.GetDiagnosticId();
        public string Title => RefactoringMessages.ConditionalComplexityTitle();
        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.MethodDeclaration};    

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax)node;
            var complexity = CalculateComplexity(methodNode);

            return complexity > ConditionalComplexityThreshold ?
               CreateFailedDiagnosticResult(methodNode, complexity) :
               DiagnosticInfo.CreateSuccessfulResult(complexity);
        }

        private static DiagnosticInfo CreateFailedDiagnosticResult(MethodDeclarationSyntax methodNode, int complexity) => 
            DiagnosticInfo.CreateFailedResult
                (RefactoringMessages.ConditionalComplexityMessage(complexity), complexity, methodNode.Identifier.GetLocation());

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) =>
            new[] {node};
    
        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            token.Parent;
		
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

        private static int CalculateComplexity(SyntaxNode methodNode)
        {
            var visitor = new ConditionalComplexityVisitor();
            return visitor.Visit(methodNode);
        }
    }
}
