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

        private static bool IsBooleanTree(SyntaxNode syntaxNode, Compilation compilation)
        {
            var semanticModel = compilation.GetSemanticModel(syntaxNode.SyntaxTree);
            var treeType = semanticModel.GetTypeInfo(syntaxNode).Type;
            return treeType.IsValueType && treeType.Name == "Boolean";
        }

        private static void FindBooleanComparePattern(SyntaxNode syntaxNode, int level,
            Compilation compilation)
        {
            if (syntaxNode is BinaryExpressionSyntax)
            {
                var binaryExpressionSyntax = (BinaryExpressionSyntax)syntaxNode;

                if (binaryExpressionSyntax.OperatorToken.ValueText == "==")
                {
                    var left = binaryExpressionSyntax.Left;
                    var right = binaryExpressionSyntax.Right;

                    if (IsBooleanTree(left, compilation) && IsBooleanTree(right, compilation) &&
                        (right.ToString() == "true" || right.ToString() == "false"))
                    {
                        Console.WriteLine("b == True, b == False detected");
                    }
                }
            }

            foreach (var child in syntaxNode.ChildNodes())
            {
                FindBooleanComparePattern(child, level + 1, compilation);
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

            FindBooleanComparePattern(root, 0, compilation);

            Console.ReadKey(true);
        }
    }
}
