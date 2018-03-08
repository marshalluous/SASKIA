using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RHW
{
    public class RoslynEngine
    {
        public List<string> GetClassNames(SyntaxTree syntaxTree)
        {
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            var classVisitor = new RoslynClassVisitor();
            classVisitor.Visit(root);
            return classVisitor.Classes; // list of classes in your solution
        }

        public CSharpCompilation GetCompilation(SyntaxTree syntaxTree)
        {
            return CSharpCompilation.Create("Test")
                 .AddReferences(references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) })
                .AddSyntaxTrees(syntaxTree);
        }

        public SyntaxTree GetSyntaxTree(string filename)
        {
            var source = File.ReadAllText(GetSourceFile(filename));
            return CSharpSyntaxTree.ParseText(source);
        }

        private string GetSourceFile(string filename)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return $"{assemblyPath}\\..\\..\\{filename}";
        }


    }
}
