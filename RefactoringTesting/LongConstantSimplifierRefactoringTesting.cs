using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.LongConstantSimplifier;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class LongConstantSimplifierRefactoringTesting
    {
        [TestMethod]
        public void AdditionIntegerTest()
        {
            var source = MethodSource("var x = 567 +  4;");
            TestCodeFix(source, "571L");
        }

        [TestMethod]
        public void AdditionLongTest()
        {
            var source = MethodSource("var x = 567l + 4l;");
            TestCodeFix(source, "571L");
            source = MethodSource("var x = 567l + 4;");
            TestCodeFix(source, "571L");
            source = MethodSource("var x = 567 + 4l;");
            TestCodeFix(source, "571L");
        }

        [TestMethod]
        public void SubtractionIntegerTest()
        {
            var source = MethodSource("var y = 45 - 12 ;");
            TestCodeFix(source, "33L");
        }

        [TestMethod]
        public void SubtractionLongTest()
        {
            var source = MethodSource("var x = 4l - 6;");
            TestCodeFix(source, "-2L");
        }

        [TestMethod]
        public void MultiplicationIntegerTest()
        {
            var source = MethodSource("var z = 4 * 12L;");
            TestCodeFix(source, "48L");
        }

        [TestMethod]
        public void MultiplicationLongTest()
        {
            var source = MethodSource("var z = 4 * 12L;");
            TestCodeFix(source, "48L");
        }

        [TestMethod]
        public void ModuloLongTest()
        {
            var source = MethodSource("var x = 334L % 12;");
            TestCodeFix(source, "10L");
        }

        [TestMethod]
        public void ModuloByZeroTest()
        {
            var source = MethodSource("var x = 34 % 0L;");
            TestCodeFix(source, string.Empty);
        }
        
        [TestMethod]
        public void DivisionTest()
        {
            var source = MethodSource(" var x = 40L / 12l;");
            TestCodeFix(source, "3L");
        }

        [TestMethod]
        public void DivisionByZeroTest()
        {
            var source = MethodSource(" var x = 40l / 0;");
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
            TestCodeFix("var x = +12L;", "12L");
            TestCodeFix("var x = +12;", "12L");
        }

        [TestMethod]
        public void UnaryMinusTest()
        {
            TestCodeFix("var x = -12;", "-12L");
            TestCodeFix("var x = -12L;", "-12L");
        }

        [TestMethod]
        public void BitInversionTest()
        {
            TestCodeFix("var x = ~34L;", ~34 + "L");
        }

        private static void TestCodeFix(string inputCode, string expectedNodeText)
        {
            TestHelper.TestCodeFix<BinaryExpressionSyntax>(new LongConstantSimplifierRefactoring(), inputCode, expectedNodeText, FindNode);
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

        private static string MethodSource(string code)
        {
            return "class A { public void B() { " + code + " } }";
        }
    }
}
