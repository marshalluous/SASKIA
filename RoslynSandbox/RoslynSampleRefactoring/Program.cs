using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynSampleRefactoring
{
    public sealed class Program
    {
        private static string GetSourceFilePath()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return $"{assemblyPath}\\..\\..\\source.txt";
        }
        
        private static void FindBooleanComparePattern(SyntaxNode syntaxNode, int level)
        {
            //Console.Write(new string(' ', level));
            //Console.WriteLine(syntaxNode.GetType().Name);

            if (syntaxNode is BinaryExpressionSyntax)
            {
                var binaryExpressionSyntax = (BinaryExpressionSyntax)syntaxNode;

                if( binaryExpressionSyntax.OperatorToken.ValueText == "==")
                {
                    Console.WriteLine(binaryExpressionSyntax.Left.GetType());
                    Console.WriteLine(binaryExpressionSyntax.Right.GetType());
                }
            }

            foreach (var child in syntaxNode.ChildNodes())
            {
                FindBooleanComparePattern(child, level + 1);
            }
        }

        public static void Main()
        {
            var source = File.ReadAllText(GetSourceFilePath());
            var tree = CSharpSyntaxTree.ParseText(source);

            var compilation = CSharpCompilation.Create("Test")
                 .AddReferences(references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) })
                .AddSyntaxTrees(tree);
        
            var root = (CompilationUnitSyntax)tree.GetRoot();

            FindBooleanComparePattern(root, 0);
            
            Console.ReadKey(true);
        }
    }
}
