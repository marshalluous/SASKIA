using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.BooleanConstantSimplifier;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class BooleanConstantSimplifierRefactoringTesting
    {
        [TestMethod]
        public void MixedBooleanConstantExpressionTest()
        {
            const string source = "var x = true && false || !true;";
            TestCodeFix(source, "false");
        }

        [TestMethod]
        public void SimpleAndExpressionTest()
        {
            const string source = "var y = true && false;";
            TestCodeFix(source, "false");
        }

        [TestMethod]
        public void SimpleOrExpressionTest()
        {
            const string source = "var z = true || false;";
            TestCodeFix(source, "true");
        }

        [TestMethod]
        public void SimpleNotExpressionTest()
        {
            const string source = "var z = !false;";
            TestCodeFix(source, "true");
        }


        [TestMethod]
        public void SimpleBracketExpressionTest()
        {
            const string source = "var z = !(false || true);";
            TestCodeFix(source, "false");
        }

        [TestMethod]
        public void DoubleNotTest()
        {
            const string source = "bool X() { return !!false; }";
            TestCodeFix(source, "false");
        }

        [TestMethod]
        public void BracketDoubleNotTest()
        {
            const string source = "bool X() { return !(!true); }";
            TestCodeFix(source, "true");
        }

        private static void TestCodeFix(string inputCode, string expectedNodeText)
        {
            var node = Compile(inputCode);
            var refactoring = new BooleanConstantSimplifierRefactoring();
            node = FindNode(node);
            Assert.IsNotNull(node);
            var resultNode = refactoring.GetFixableNodes(node).First();
            Assert.AreEqual(expectedNodeText, resultNode.ToString());
        }

        private static SyntaxNode FindNode(SyntaxNode node)
        {
            if (node is BinaryExpressionSyntax || node is PrefixUnaryExpressionSyntax)
                return node;

            return node.ChildNodes().Select(FindNode).FirstOrDefault(foundNode => foundNode != null);
        }

        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
    }
}
