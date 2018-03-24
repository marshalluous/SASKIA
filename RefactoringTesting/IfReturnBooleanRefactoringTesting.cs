using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.IfReturnBoolean;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class IfReturnBooleanRefactoringTesting
    {
        [TestMethod]
        public void TestReturnBoolean()
        {
            var source = GetCodeTemplate("if (true == false) { return true; } else { return false; }");
            TestCodeFix(source, "return true == false;");
        }

        [TestMethod]
        public void TestReturnFalseReturnTrueSample()
        {
            var source = GetCodeTemplate("if (true == false) { return false; } else { return true; }");
            TestCodeFix(source, "return !(true == false);");
        }

        [TestMethod]
        public void TestMethodCallCondition()
        {
            var source = GetCodeTemplate("if (A()) { return false; } else { return true; }");
            TestCodeFix(source, "return !A();");

            source = GetCodeTemplate("int x = 4; if (nameof(x) == \"x\") { return false; } else {return true; }");
            TestCodeFix(source, "return !(nameof(x) == \"x\");");
        }

        [TestMethod]
        public void TestIntegerComparisonCondition()
        {
            var source = GetCodeTemplate("if (4 < 12) { return false; } else return true;");
            TestCodeFix(source, "return !(4 < 12);");
        }
        
        private static string GetCodeTemplate(string code)
        {
            return "class X { public bool Y() {" + code + "}}";
        }
        
        private static void TestCodeFix(string inputCode, string expectedNodeText)
        {
            var node = Compile(inputCode);
            var refactoring = new IfReturnBooleanRefactoring();
            node = FindNode(node);
            Assert.IsNotNull(node);
            var resultNode = refactoring.ApplyFix(node).First();
            Assert.AreEqual(expectedNodeText, resultNode.ToString());
        }

        private static SyntaxNode FindNode(SyntaxNode node)
        {
            if (node is IfStatementSyntax)
            {
                return node;
            }

            return node.ChildNodes().Select(FindNode)
                .FirstOrDefault(foundNode => foundNode != null);
        }

        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
    }
}