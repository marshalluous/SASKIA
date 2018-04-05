using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Refactorings.BooleanConstantComparison;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class BooleanConstantComparisonRefactoringTesting
    {
        [TestMethod]
        public void CompareWithTrueLiteralOnRightHandSideTest()
        {
            TestCodeFix("var b = x == true;", "x");
        }

        [TestMethod]
        public void CompareWithFalseLiteralOnRightHandSideTest()
        {
            TestCodeFix("var b = x == false;", "!x");
        }

        [TestMethod]
        public void EqualsEqualsNestedInEqualsEqualsTest()
        {
            TestCodeFix("var x = 4 == 12 == true;", "4 == 12");
        }

        [TestMethod]
        public void CompareWithTrueLiteralOnLeftHandSide()
        {
            TestCodeFix("var b = true == x", "x");
        }

        [TestMethod]
        public void CompareWithFalseLiteralOnLeftHandSide()
        {
            TestCodeFix("var b = false == x", "!x");
        }

        [TestMethod]
        public void NotEqualsCompareWithTrueLiteralOnRightHandSideTest()
        {
            TestCodeFix("var b = x != true;", "!x");
        }

        [TestMethod]
        public void NotEqualsCompareWithFalseLiteralOnRightHandSideTest()
        {
            TestCodeFix("var b = x != false;", "x");
        }

        [TestMethod]
        public void NotEqualsCompareWithTrueLiteralOnLeftHandSide()
        {
            TestCodeFix("var b = true != x", "!x");
        }

        [TestMethod]
        public void NotEqualsCompareWithFalseLiteralOnLeftHandSide()
        {
            TestCodeFix("var b = false != x", "x");
        }

        [TestMethod]
        public void MethodCallEqualsEqualsComparison()
        {
            TestCodeFix("var k = true == A();", "A()");
            TestCodeFix("var k = A() == false;", "!A()");
        }

        [TestMethod]
        public void MethodCallNotEqualsComparison()
        {
            TestCodeFix("var k = false != A();", "A()");
            TestCodeFix("var k = A() != true;", "!A()");
        }
        
        [TestMethod]
        public void ComplexExpressionEqualsEqualsTrue()
        {
            TestCodeFix("var k = 4 < 5 == true;", "4 < 5");
        }

        [TestMethod]
        public void ComplexExpressionEqualsEqualsFalse()
        {
            TestCodeFix("var k = 4 < 12 == false;", "!(4 < 12)");
        }

        [TestMethod]
        public void TestSpecialNameofComparison()
        {
            TestCodeFix("var x = ((nameof(x) == \"x\") == false);", "!(nameof(x) == \"x\")");
        }

        private static void TestCodeFix(string inputCode, string expectedNodeText)
        {
            TestHelper.TestCodeFix<BinaryExpressionSyntax>(new BooleanConstantComparisonRefactoring(), inputCode, expectedNodeText);
        }
    }
}