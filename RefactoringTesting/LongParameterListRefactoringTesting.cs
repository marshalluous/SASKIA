using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.LongParameterList;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public class LongParameterListRefactoringTesting
    {
        [TestMethod]
        public void NoParameterMethodTest()
        {
            TestParameterList("public void X() { }", false, 0);
        }

        [TestMethod]
        public void OneParameterMethodTest()
        {
            TestParameterList("public void X(int x) { }", false, 1);
        }

        [TestMethod]
        public void TwoParameterMethodTest()
        {
            TestParameterList("public void X(int x, float z) { }", false, 2);
        }

        [TestMethod]
        public void ThreeParameterMethodTest()
        {
            TestParameterList("public void X(int x, int y, int z) { }", false, 3);
        }

        [TestMethod]
        public void FourParameterMethodTest()
        {
            TestParameterList("public void X(int x, int y, int z, int a) { }", true, 4);
        }

        [TestMethod]
        public void FiveParameterMethodTest()
        {
            TestParameterList("public void X(int x, int y, int z, int a, bool b) { }", true, 5);
        }

        private static void TestParameterList(string inputCode, bool diagnosticFound, int metricValue)
        {
            TestHelper.TestMetric<MethodDeclarationSyntax>(new LongParameterListRefactoring(), inputCode, diagnosticFound, metricValue);
        }
    }
}
