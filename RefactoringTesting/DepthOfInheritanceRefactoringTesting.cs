using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Helper;
using Refactoring.Refactorings.DepthOfInheritance;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class DepthOfInheritanceRefactoringTesting
    {
        [TestMethod]
        public void TestObjectExtendingClass()
        {
            TestDepthOfInheritance("class Fish : object {}", false, 1);
        }

        [TestMethod]
        public void TestNonExtendingClass()
        {
            TestDepthOfInheritance("class Fish {}", false, 1);
        }

        [TestMethod]
        public void TestInterfaceImplementingClass()
        {
            TestDepthOfInheritance("interface A {} class B : A {}", false, 1);
            TestDepthOfInheritance("interface A {} class B : object, A{}", false, 1);
        }

        [TestMethod]
        public void TestSimpleExtendingClass()
        {
            TestDepthOfInheritance("class C : A, B {} class A {} interface B {}", false, 2);
        }

        [TestMethod]
        public void TestDepthOfInheritanceSmellMetric()
        {
            const string source = "class E : D {} class A : object {} class B : A {} class C : B{} class D : C {}";
            TestDepthOfInheritance(source, true, 5);
        }

        [TestMethod]
        public void TestDepthOfInheritanceSmellDiagnostic()
        {
            const string source = "class E : D {} class A : object {} class B : A {} class C : B{} class D : C {}";
            TestDiagnostic(source, RefactoringMessages.DepthOfInheritanceMessage("E", 5));
        }

        private static void TestDepthOfInheritance(string inputCode, bool diagnosticFound, int metricValue)
        {
            TestHelper.TestMetric<ClassDeclarationSyntax>(new DepthOfInheritanceRefactoring(), inputCode, diagnosticFound, metricValue);
        }

        private static void TestDiagnostic(string inputCode, string outputMessage)
        {
            TestHelper.TestDiagnosticResult<ClassDeclarationSyntax>(new DepthOfInheritanceRefactoring(), inputCode, outputMessage);
        }
    }
}