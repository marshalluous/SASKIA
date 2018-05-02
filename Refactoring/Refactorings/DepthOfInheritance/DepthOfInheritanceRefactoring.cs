using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.DepthOfInheritance
{
    public sealed class DepthOfInheritanceRefactoring : IRefactoring
    {
        private const int ThresholdDepthOfInheritance = 4;

        public string DiagnosticId => RefactoringId.DepthOfInheritance.GetDiagnosticId();
        public string Title => RefactoringMessageFactory.DepthOfInheritanceTitle();
        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.ClassDeclaration};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var classNode = (ClassDeclarationSyntax) node;
            var classSymbol = SemanticSymbolBuilder.GetTypeSymbol(classNode);
            var depthOfInheritance = CalculateDepthOfInheritance(classSymbol);

            return depthOfInheritance > ThresholdDepthOfInheritance
                ? DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory.DepthOfInheritanceMessage(classSymbol.Name, depthOfInheritance),
                depthOfInheritance, classNode.Identifier.GetLocation())
                : DiagnosticInfo.CreateSuccessfulResult(depthOfInheritance);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) =>
            new[] {node};

        public SyntaxNode GetReplaceableNode(SyntaxToken token) => token.Parent;
		
        private static int CalculateDepthOfInheritance(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null || typeSymbol.Name == "Object")
                return 0;
            return 1 + CalculateDepthOfInheritance(typeSymbol.BaseType);
        }
		
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);
	}
}