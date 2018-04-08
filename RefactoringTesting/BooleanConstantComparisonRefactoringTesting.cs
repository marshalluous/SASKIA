using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Refactorings.BooleanConstantComparison;
using RefactoringTesting.Helper;
using Refactoring.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class BooleanConstantComparisonRefactoringTesting
    {
        [TestMethod]
        public void CompareWithTrueLiteralOnRightHandSideCodeFixTest()
        {
            TestCodeFix("var b = x == true;", "x");
        }
        
        [TestMethod]
        public void CompareWithTrueLiteralOnRightHandSideDiagnosticTest()
        {
            TestDiagnosticResult("var b = x == true;", RefactoringMessageFactory.BooleanConstantComparisonMessage(true));
        }

        [TestMethod]
        public void CompareWithFalseLiteralOnRightHandSideCodeFixTest()
        {
            TestCodeFix("var b = x == false;", "!x");
        }

        [TestMethod]
        public void CompareWithFalseLiteralOnRightHandSideDiagnosticTest()
        {
            TestDiagnosticResult("var b = x == false;", RefactoringMessageFactory.BooleanConstantComparisonMessage(false));
        }

        [TestMethod]
        public void EqualsEqualsNestedInEqualsEqualsCodeFixTest()
        {
            TestCodeFix("var x = 4 == 12 == true;", "4 == 12");
        }

        [TestMethod]
        public void EqualsEqualsNestedInEqualsEqualsDiagnosticTest()
        {
            TestDiagnosticResult("var x = 4 == 12 == true;", RefactoringMessageFactory.BooleanConstantComparisonMessage(true));
        }

        [TestMethod]
        public void CompareWithTrueLiteralOnLeftHandSideCodeFixTest()
        {
            TestCodeFix("var b = true == x", "x");
        }
        
        [TestMethod]
        public void CompareWithTrueLiteralOnLeftHandSideDiagnosticTest()
        {
            TestDiagnosticResult("var b = true == x", RefactoringMessageFactory.BooleanConstantComparisonMessage(true));
        }

        [TestMethod]
        public void CompareWithFalseLiteralOnLeftHandSideCodeFixTest()
        {
            TestCodeFix("var b = false == x", "!x");
        }

        [TestMethod]
        public void NotEqualsCompareWithTrueLiteralOnRightHandSideCodeFixTest()
        {
            TestCodeFix("var b = x != true;", "!x");
        }

        [TestMethod]
        public void NotEqualsCompareWithFalseLiteralOnRightHandSideCodeFixTest()
        {
            TestCodeFix("var b = x != false;", "x");
        }

        [TestMethod]
        public void NotEqualsCompareWithTrueLiteralOnLeftHandSideCodeFixTest()
        {
            TestCodeFix("var b = true != x", "!x");
        }

        [TestMethod]
        public void NotEqualsCompareWithFalseLiteralOnLeftHandSideCodeFixTest()
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
        public void MethodCallNotEqualsComparisonDiagnosticTest()
        {
            TestDiagnosticResult("var k = false != A();", RefactoringMessageFactory.BooleanConstantComparisonMessage(false));
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

        private static void TestDiagnosticResult(string inputCode, string diagnosticMessage)
        {
            TestHelper.TestDiagnosticResult<BinaryExpressionSyntax>(new BooleanConstantComparisonRefactoring(), inputCode, diagnosticMessage);
        }
    }
}