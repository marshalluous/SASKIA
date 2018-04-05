using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.LackOfCohesion;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class LackOfCohesionRefactoringTesting
    {
        [TestMethod]
        public void OneMethodOneFieldTest()
        {
            CheckLackOfCohesion("class A { int x; public void X() { x = 12; } }", false, 0);
        }

        [TestMethod]
        public void NoMethodTest()
        {
            CheckLackOfCohesion("class A { int x; int y; }", false, 0);
        }

        [TestMethod]
        public void SimpleLackOfCohesionTest()
        {
            CheckLackOfCohesion("class A { int x; public int X() { return x; }" +
                                "int y; public int Y() { return y; }" +
                                "}", true, 1.0d);
        }

        [TestMethod]
        public void NoLackOfCohesionTest()
        {
            CheckLackOfCohesion("class A { int x; int y; int z; public void X() { x = 12; y = 12; }" +
                                "public void Y() { y = 12; z = 4; } }", true, 0.66666666666666674d);
        }

        [TestMethod]
        public void GetterSetterTest()
        {
            CheckLackOfCohesion("class X { int x; int y; public int X => x; public int Y => y; }",
                false, 0);
        }

        [TestMethod]
        public void EditorSample1Test()
        {
            CheckLackOfCohesion(@"public class X
                {
                    private int x;
                    private int a;

                    public void A()
                    {
                        Console.WriteLine(x + a);
                    }

                    public void B()
                    {
                        x = 12;
                        a = 4;
                    }
                }", false, 0d);
        }


        private static void CheckLackOfCohesion(string inputCode, bool diagnosticFound, double metricValue)
        {
            TestHelper.TestMetric<ClassDeclarationSyntax>(new LackOfCohesionRefactoring(), inputCode, diagnosticFound, metricValue);
        }
    }
}
