using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.IntegerConstantSimplifier;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class IntegerConstantSimplifierRefactoringTesting
    {
        [TestMethod]
        public void AdditionTest()
        {
            var source = MethodSource("var x = 567 +  4;");
            TestCodeFix(source, "571");
        }

        [TestMethod]
        public void SubtractionTest()
        {
            var source = MethodSource("var y = 45 - 12 ;");
            TestCodeFix(source, "33");
        }

        [TestMethod]
        public void MultiplicationTest()
        {
            var source = MethodSource("var z = 4 * 12;");
            TestCodeFix(source, "48");
        }

        [TestMethod]
        public void ModuloTest()
        {
            var source = MethodSource("var x = 334 % 12;");
            TestCodeFix(source, "10");
        }

        [TestMethod]
        public void ModuloByZeroTest()
        {
            var source = MethodSource("var x = 34 % 0;");
            TestCodeFix(source, string.Empty);
        }

        [TestMethod]
        public void BitShiftOperationTest()
        {
            var source = MethodSource("var x = 4 << 3;");
            TestCodeFix(source, "32");
            source = MethodSource("var x = 12 >> 3;");
            TestCodeFix(source, "1");
        }
      
        [TestMethod]
        public void DivisionTest()
        {
            var source = MethodSource(" var x = 40 / 12;");
            TestCodeFix(source, "3");
        }

        [TestMethod]
        public void DivisionByZeroTest()
        {
            var source = MethodSource(" var x = 40 / 0;");
            TestCodeFix(source, string.Empty);
        }

        [TestMethod]
        public void IncrementDecrementTest()
        {
            TestCodeFix(MethodSource("var x = 4; y = ++x;"), string.Empty);
            TestCodeFix(MethodSource("var x = 12; y = --x;"), string.Empty);
            TestCodeFix(MethodSource("var x = 3; y = x--;"), string.Empty);
            TestCodeFix(MethodSource("var x = 8; y = x++;"), string.Empty);
        }

        [TestMethod]
        public void UnaryPlusTest()
        {
            TestCodeFix("var x = +12;", "12");
        }

        [TestMethod]
        public void UnaryMinusTest()
        {
            TestCodeFix("var x = -12;", "-12");
        }

        [TestMethod]
        public void BitInversionTest()
        {
            TestCodeFix("var x = ~34;", (~34).ToString());
        }

        private static void TestCodeFix(string inputCode, string expectedNodeText)
        {
            var node = Compile(inputCode);
            var refactoring = new IntegerConstantSimplifierRefactoring();
            node = FindNode(node);
            Assert.IsNotNull(node);
            var resultNode = refactoring.GetFixableNodes(node).First();
            Assert.AreEqual(expectedNodeText, resultNode.ToString());
        }

        private static SyntaxNode FindNode(SyntaxNode node)
        {
            if (node is PostfixUnaryExpressionSyntax || node is PrefixUnaryExpressionSyntax || node is BinaryExpressionSyntax)
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
        
        private static string MethodSource(string code)
        {
            return "class A { public void B() { " + code + " } }";
        }
    }
}
