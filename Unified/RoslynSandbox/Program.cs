using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace RoslynSandbox
{
    public static class Program
    {
        private static void PrintTree(SyntaxNode member, int depth = 0)
        {
            if (member is NamespaceDeclarationSyntax)
            {
                PrintNamespaceNode((NamespaceDeclarationSyntax)member, depth + 1);
            }
            else if (member is ClassDeclarationSyntax)
            {
                PrintClassNode((ClassDeclarationSyntax)member, depth + 1);
            }
            else if (member is MethodDeclarationSyntax)
            {
                PrintMethodNode((MethodDeclarationSyntax)member, depth + 1);
            }
        }

        private static void PrintNamespaceNode(NamespaceDeclarationSyntax member, int depth)
        {
            Console.WriteLine($"NAMESPACE {member.Name}");

            foreach (var childMember in member.ChildNodes())
            {
                PrintTree(childMember, depth);
            }
        }

        private static void PrintClassNode(ClassDeclarationSyntax member, int depth)
        {
            Console.WriteLine($"  CLASS {member.Identifier}");

            foreach (var childMember in member.ChildNodes())
            {
                PrintTree(childMember, depth);
            }
        }
        
        private static void PrintMethodNode(MethodDeclarationSyntax member, int depth)
        {
            Console.WriteLine($"    METHOD {member.Identifier} RETURNS {member.ReturnType}");
        }

        public static void Main()
        {
            var tree = CSharpSyntaxTree.ParseText("namespace A { public class B {" +
                " public int X() { return 12;" +
                " } public void C() {} } " +
                "public class D { public void A() {} }" +
                "} ");
            
            var compilation = CSharpCompilation.Create("Test")
                 .AddReferences(references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) })
                .AddSyntaxTrees(tree);

            var model = compilation.GetSemanticModel(tree, false);

            var root = (CompilationUnitSyntax) tree.GetRoot();
            
            foreach (var member in root.Members)
            {
                PrintTree(member);
            }
            
            Console.ReadKey(true);
        }
    }
}