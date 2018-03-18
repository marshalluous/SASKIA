using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.DepthOfInheritance
{
    public sealed class DepthOfInheritanceRefactoring : IRefactoring
    {
        private const int ThresholdDepthOfInheritance = 4;

        public string DiagnosticId => "SASKIA500";

        public string Title => DiagnosticId;

        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.ClassDeclaration};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var classNode = (ClassDeclarationSyntax) node;
            var compilation = CSharpCompilation.Create("MyCompilation", new[] { node.SyntaxTree });
            var model = compilation.GetSemanticModel(node.SyntaxTree);

            var classSema = node.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier == classNode.Identifier);
            var sym = model.GetDeclaredSymbol(classSema);

            var depthOfInheritance = CalculateDepthOfInheritance(sym);

            return depthOfInheritance > ThresholdDepthOfInheritance
                ? DiagnosticInfo.CreateFailedResult("DIT alarm", depthOfInheritance)
                : DiagnosticInfo.CreateSuccessfulResult(depthOfInheritance);
        }

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node) =>
            new[] {node};

        public SyntaxNode GetReplaceableNode(SyntaxToken token) => token.Parent;
        
        private static int CalculateDepthOfInheritance(ITypeSymbol typeSymbol)
        {
            if (typeSymbol.Name == "Object")
                return 0;
            
            return 1 + CalculateDepthOfInheritance(typeSymbol.BaseType);
        }
    }
}