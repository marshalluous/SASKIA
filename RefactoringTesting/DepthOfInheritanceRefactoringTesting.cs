using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.DepthOfInheritance;

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
        public void TestDepthOfInheritanceSmell()
        {
            const string source = "class E : D {} class A : object {} class B : A {} class C : B{} class D : C {}";
            TestDepthOfInheritance(source, true, 5);
        }

        private static void TestDepthOfInheritance(string source, bool toHighDepthOfInheritance, int depthOfInheritance)
        {
            var node = Compile(source);
            node = FindNode(node);
            Assert.IsNotNull(node);
            var refactoring = new DepthOfInheritanceRefactoring();
            var diagnosticInfo = refactoring.DoDiagnosis(node);
            Assert.AreEqual(toHighDepthOfInheritance, diagnosticInfo.DiagnosticFound);
            Assert.AreEqual(depthOfInheritance, diagnosticInfo.AdditionalInformation);
        }

        private static SyntaxNode FindNode(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax ?
                node : 
                node.ChildNodes().Select(FindNode).FirstOrDefault(foundNode => foundNode != null);
        }

        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
    }
}
