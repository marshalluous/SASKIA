using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.SyntaxTreeHelper
{
	public static class SemanticSymbolBuilder
	{
	    private const string CompilationUnitName = "CompilationUnit";

        public static SemanticModel GetSemanticModel(BaseTypeDeclarationSyntax classNode)
        {
            var classSyntaxTree = classNode.SyntaxTree;
            return Compilation(classSyntaxTree).GetSemanticModel(classNode.SyntaxTree);
        }

        private static CSharpCompilation Compilation(SyntaxTree classSyntaxTree) => 
            CSharpCompilation.Create(CompilationUnitName, new[] { classSyntaxTree }, new[] { CoreLibrary() });

	    public static ITypeSymbol GetTypeSymbol(BaseTypeDeclarationSyntax classNode, SemanticModel model = null)
        {
            model = model ?? GetSemanticModel(classNode);
            return model.GetDeclaredSymbol(GetClassSemanticNode(classNode));
        }

        private static ClassDeclarationSyntax GetClassSemanticNode(BaseTypeDeclarationSyntax classNode) => 
            classNode.SyntaxTree.GetRoot().DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault(node => node.Identifier == classNode.Identifier);

	    private static PortableExecutableReference CoreLibrary() =>
	        MetadataReference.CreateFromFile(CorePath());

	    private static string CorePath() =>
	        typeof(object).GetTypeInfo().Assembly.Location;
    }
}