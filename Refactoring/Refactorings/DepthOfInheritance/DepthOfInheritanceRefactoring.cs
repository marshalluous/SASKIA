using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.DepthOfInheritance
{
    public sealed class DepthOfInheritanceRefactoring : IRefactoring
    {
        private const int ThresholdDepthOfInheritance = 4;

        public string DiagnosticId => RefactoringId.DepthOfInheritance.GetDiagnosticId();
        public string Title => RefactoringMessages.DepthOfInheritanceTitle();
        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.ClassDeclaration};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var classNode = (ClassDeclarationSyntax)node;
            var classSymbol = SemanticSymbolBuilder.GetTypeSymbol(classNode);
            var depthOfInheritance = CalculateDepthOfInheritance(classSymbol);
            return CreateDiagnosticResult(classNode, classSymbol, depthOfInheritance);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) => null;
        public SyntaxNode GetReplaceableNode(SyntaxToken token) => token.Parent;
		
        private static int CalculateDepthOfInheritance(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null || typeSymbol.Name == "Object")
                return 0;
            return 1 + CalculateDepthOfInheritance(typeSymbol.BaseType);
        }
		
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);
        
        private static DiagnosticInfo CreateDiagnosticResult(BaseTypeDeclarationSyntax classNode, ISymbol classSymbol, int depthOfInheritance) =>
            depthOfInheritance > ThresholdDepthOfInheritance
                ? CreateFailedDiagnosticResult(classNode, classSymbol, depthOfInheritance)
                : DiagnosticInfo.CreateSuccessfulResult(depthOfInheritance);

        private static DiagnosticInfo CreateFailedDiagnosticResult(BaseTypeDeclarationSyntax classNode, ISymbol classSymbol, int depthOfInheritance) =>
            CreateFailedDiagnosticMessage(classNode, classSymbol, depthOfInheritance);

        private static DiagnosticInfo CreateFailedDiagnosticMessage(BaseTypeDeclarationSyntax classNode, ISymbol classSymbol, int depthOfInheritance) =>
            DiagnosticInfo.CreateFailedResult(RefactoringMessages.DepthOfInheritanceMessage(classSymbol.Name, depthOfInheritance),
                depthOfInheritance, classNode.Identifier.GetLocation());
    }
}