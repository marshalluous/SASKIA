using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper
{
	public static class SemanticSymbolBuilder
	{
		public static SemanticModel GetSemanticModel(BaseTypeDeclarationSyntax classNode)
		{
		    var corePath = typeof(object).GetTypeInfo().Assembly.Location;
		    var mscorlib = MetadataReference.CreateFromFile(corePath);
			var classSyntaxTree = classNode.SyntaxTree;
			var compilation = CSharpCompilation.Create("CompilationUnit", new[] { classSyntaxTree }, references: new [] { mscorlib });
			return compilation.GetSemanticModel(classNode.SyntaxTree);
		}

		public static ITypeSymbol GetTypeSymbol(BaseTypeDeclarationSyntax classNode, SemanticModel model = null)
		{
			if (model == null)
				model = GetSemanticModel(classNode);

			var classSemanticNode = classNode.SyntaxTree.GetRoot().DescendantNodes()
				.OfType<ClassDeclarationSyntax>()
				.FirstOrDefault(node => node.Identifier == classNode.Identifier);

			return model.GetDeclaredSymbol(classSemanticNode);
		}
	}
}
