using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.DeMorganSimplifier;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class DeMorganSimplifierRefactoringTesting
    {
        [TestMethod]
        public void TestSimpleOrExpression()
        {
            TestCodeFix("if (!(x == 4 || x == 12)) {}", "!(x == 4) && !(x == 12)");
        }

        [TestMethod]
        public void TestSimpleAndExpression()
        {
            TestCodeFix("!(x != 4 && x != 5)", "!(x != 4) || !(x != 5)");
        }

        [TestMethod]
        public void TestTripleOrExpression()
        {
            TestCodeFix("!(x == 4 || x == 12 || x == 3)", "!(x == 4 || x == 12) && !(x == 3)");
        }

        [TestMethod]
        public void TestTripleOrExpressionApplyTwice()
        {
            TestCodeFix("!(x == 4 || x == 12) && !(x == 3)", "!(x == 4) && !(x == 12)");
        }


        [TestMethod]
        public void TestTripleAndExpression()
        {
            TestCodeFix("!(x == 4 && x == 12 && x == 3)", "!(x == 4 && x == 12) || !(x == 3)");
        }

        [TestMethod]
        public void TestTripleAndExpressionApplyTwice()
        {
            TestCodeFix("!(x == 4 && x == 12) || !(x == 3)", "!(x == 4) || !(x == 12)");
        }
        
        private static void TestCodeFix(string sourceCode, string expectedNodeText)
        {
            var source = "public class X { public void A() { if (" + sourceCode + ") {} } private int x; }";
            TestHelper.TestCodeFix<PrefixUnaryExpressionSyntax>
                (new DeMorganSimplifierRefactoring(), source, expectedNodeText);
        }
    }
}
