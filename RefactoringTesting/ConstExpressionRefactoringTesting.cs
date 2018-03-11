using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring;
using System.Linq;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class ConstExpressionRefactoringTesting
    {
        [TestMethod]
        public void AdditionTest()
        {
            TestCodeFix<BinaryExpressionSyntax>(MethodTemplateCode("Console.WriteLine(12 + 4);"), "16");
        }

        [TestMethod]
        public void MultiplicationTest()
        {
            TestCodeFix<BinaryExpressionSyntax>(MethodTemplateCode("A(12 * 5);"), "60");
        }

        [TestMethod]
        public void LessThanGreaterThanTest()
        {
            TestCodeFix<BinaryExpressionSyntax>(MethodTemplateCode("A(12 < 45);"), "true");
            TestCodeFix<BinaryExpressionSyntax>(MethodTemplateCode("B(3 > 12);"), "false");
        }

        [TestMethod]
        public void NotTest()
        {
            TestCodeFix<BinaryExpressionSyntax>(MethodTemplateCode("A(!(3 < 12));"), "true");
            TestCodeFix<PrefixUnaryExpressionSyntax>(MethodTemplateCode("bool b = !false;"), "true");
        }

        private static string MethodTemplateCode(string code)
        {
            return "namespace A { class B { public void C() {" + code + "}}}";
        }
        
        private static void TestCodeFix<T>(string inputCode, string expectedNodeText)
        {
            var node = Compile(inputCode);
            var refactoring = new ConstExpressionRefactoring();
            node = FindNodeOfType<T>(node);
            Assert.IsNotNull(node);
            var resultNode = refactoring.ApplyFix(node).First();
            Assert.AreEqual(expectedNodeText, resultNode.ToString());
        }
        
        private static SyntaxNode FindNodeOfType<T>(SyntaxNode node)
        {
            if (node is T)
            {
                return node;
            }

            foreach (var childNode in node.ChildNodes())
            {
                var foundNode = FindNodeOfType<T>(childNode);

                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null;
        }

        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
    }
}
