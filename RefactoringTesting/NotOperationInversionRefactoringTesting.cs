using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.NotOperatorInversion;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class NotOperationInversionRefactoringTesting
    {
        [TestMethod]
        public void NotEqualsEqualsTest()
        {
            TestCodeFix("var x = !(4 == 12);", "4 != 12");
        }

        [TestMethod]
        public void NestedNotExpressionTest()
        {
            TestCodeFix("var x = !(!(4 != 12))", "4 != 12");
        }

        [TestMethod]
        public void NotLessThanExpressionTest()
        {
            TestCodeFix("var x = !(4 < 12)", "4 >= 12");
        }

        [TestMethod]
        public void NotGreaterThanExpressionTest()
        {
            TestCodeFix("var x = !(4 > 12)", "4 <= 12");
        }

        [TestMethod]
        public void NotLessThanEqualsTest()
        {
            TestCodeFix("var x = !(4 <= 12)", "4 > 12");
        }

        [TestMethod]
        public void NotGreaterThanEqualsTest()
        {
            TestCodeFix("var x = !(4 >= 12)", "4 < 12");
        }

        [TestMethod]
        public void NestedNotLessThanExpression()
        {
            TestCodeFix("var x = !(!(4 < 10))", "4 < 10");
        }
        
        private static void TestCodeFix(string inputCode, string expectedOutput)
        {
            TestHelper.TestCodeFix<PrefixUnaryExpressionSyntax>(new NotOperatorInversionRefactoring(), inputCode, expectedOutput);
        }
    }
}
