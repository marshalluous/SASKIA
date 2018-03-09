using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynSampleAppliedRefactoring
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

        private static bool IsEqualsTree(SyntaxNode syntaxNode)
        {
            return syntaxNode is BinaryExpressionSyntax &&
                ((BinaryExpressionSyntax)syntaxNode).OperatorToken.ValueText == "==";
        }
        
        private static SyntaxNode NotNode(ExpressionSyntax syntaxNode)
        {
            return SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, syntaxNode);
        }

        private static SyntaxNode FindBooleanComparePattern(SyntaxNode syntaxNode, int level,
            Compilation compilation)
        {
            if (IsEqualsTree(syntaxNode))
            {
                var binaryExpressionSyntax = (BinaryExpressionSyntax) syntaxNode;
                var left = binaryExpressionSyntax.Left;
                var right = binaryExpressionSyntax.Right;
                    
                if (IsBooleanTree(left, compilation) && IsBooleanTree(right, compilation))
                {
                    if (left.ToString() == "true")
                    {
                        return right;
                    }
                    else if (right.ToString() == "true")
                    {
                        return left;
                    } 
                    else if (left.ToString() == "false")
                    {
                        return NotNode(right);
                    }
                    else if (right.ToString() == "false")
                    {
                        return NotNode(left);
                    }
                }
            }

            foreach (var child in syntaxNode.ChildNodes())
            {
                var newChild = FindBooleanComparePattern(child, level + 1, compilation);
                syntaxNode = syntaxNode.ReplaceNode(child, newChild);
            }
            
            return syntaxNode;
        }
        
        public static void Main()
        {
            var source = File.ReadAllText(GetSourceFilePath());

            for (var i = 0; i < 15; ++i)
            {
                var tree = CSharpSyntaxTree.ParseText(source);

                var compilation = CSharpCompilation.Create("Test")
                     .AddReferences(references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) })
                    .AddSyntaxTrees(tree);

                var root = (CompilationUnitSyntax)tree.GetRoot();

                var newTree = FindBooleanComparePattern(root, 0, compilation);
                source = newTree.ToString();
            }

            Console.WriteLine(source);
            Console.ReadKey(true);
        }
    }
}
