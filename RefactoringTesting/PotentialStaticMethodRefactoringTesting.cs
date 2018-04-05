using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.PotentialStaticMethod;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public class PotentialStaticMethodRefactoringTesting
    {
        [TestMethod]
        public void EmptyMethodTest()
        {
            TestCodeFix("class X { private void A() { } }", "private static void A()\r\n{\r\n}");
        }

        [TestMethod]
        public void NonStaticFieldAccessTest()
        {
            TestCodeFix("class X { private int x; private int A() { return x; }", string.Empty);
            TestCodeFix("class X { private int x; private int A() { return this.x; }", string.Empty);
        }

        [TestMethod]
        public void StaticFieldAccessTest()
        {
            TestCodeFix("class X { private static int x; private int A() { return x; }", "private static int A()\r\n{\r\n    return x;\r\n}");
            TestCodeFix("class X { private static int x; private int A() { return X.x; }", "private static int A()\r\n{\r\n    return X.x;\r\n}");
        }

        [TestMethod]
        public void OtherClassNonStaticFieldAccessTest()
        {
            TestCodeFix("class X { private void A() { var y = new Y(); y.a = 4; } class Y { public int a; }", "private static void A()\r\n{\r\n" +
                "    var y = new Y();\r\n    y.a = 4;\r\n}");
        }

        [TestMethod]
        public void OtherClassStaticFieldWithNonStaticFieldAccessTest()
        {
            TestCodeFix("class X { private static Y y; private void A() { y = new Y(); y.a = 4; } class Y { public int a; }", "private static void A()\r\n{\r\n" +
                "    y = new Y();\r\n    y.a = 4;\r\n}");
        }

        [TestMethod]
        public void OtherClassNonStaticFieldWithNonStaticFieldAccessTest()
        {
            TestCodeFix("class X { private Y y; private void A() { y = new Y(); y.a = 4; } class Y { public int a; }", string.Empty);
        }
        
        [TestMethod]
        public void OtherClassStaticFieldAccessTest()
        {
            TestCodeFix("class X { private void A() { Y.a = 4; } class Y { public static int a; }", "private static void A()\r\n{\r\n    Y.a = 4;\r\n}");
        }

        [TestMethod]
        public void CallStaticMethodTest()
        {
            TestCodeFix("class X { private void X() { A(); } private static void A() {} }", "private static void X()\r\n{\r\n    A();\r\n}");
            TestCodeFix("class X { private void X() { X.A(); } private static void A() {} }", "private static void X()\r\n{\r\n    X.A();\r\n}");
        }
        
        [TestMethod]
        public void CallNonMethodTest()
        {
            TestCodeFix("class X { private void X() { A(); } private void A() {} }", string.Empty);
            TestCodeFix("class K { private void X() { K.A(); } private void A() {} }", string.Empty);
        }
        
        [TestMethod]
        public void CallNonStaticMethodOnNonStaticFieldTest()
        {
            TestCodeFix("class X { private Y y = new Y(); private void A() { y.B(); } } class Y { public void B() {} }", string.Empty);
        }
        
        [TestMethod]
        public void CallNonStaticMethodOnStaticFieldTest()
        {
            TestCodeFix("class X { private static Y y = new Y(); private void A() { y.B(); } } class Y { public void B() {} }", "private static void A()\r\n{\r\n    y.B();\r\n}");
        }

        [TestMethod]
        public void CallMethodOfOtherClassTest()
        {
            TestCodeFix("class X { private void A() { Y y = new Y(); y.B(); } } class Y { public void B() {} }", "private static void A()\r\n{\r\n    Y y = new Y();\r\n    y.B();\r\n}");
        }
        
        [TestMethod]
        public void CallStaticMethodOfOtherClassTest()
        {
            TestCodeFix("class X { private void A() { Y.B(); } } class Y { public static void B() {} }", "private static void A()\r\n{\r\n    Y.B();\r\n}");
        }
        
        [TestMethod]
        public void PrivateMethodTest()
        {
            TestCodeFix("class X { private void A() { } }", "private static void A()\r\n{\r\n}");
        }

        [TestMethod]
        public void PublicMethodTest()
        {
            TestCodeFix("class X { public void A() { } }", string.Empty);
        }

        [TestMethod]
        public void InternalMethodTest()
        {
            TestCodeFix("class X { internal void A() { } }", string.Empty);
            TestCodeFix("class X { protected internal void A() { } }", string.Empty);
            TestCodeFix("class X { void A() { } }", string.Empty);
        }

        [TestMethod]
        public void ProtectedMethodTest()
        {
            TestCodeFix("class X { protected void A() { } }", string.Empty);
        }

        [TestMethod]
        public void UseOfThisTest()
        {
            TestCodeFix("class X { private X Get() { return this; } }", string.Empty);
        }

        [TestMethod]
        public void UseOfBaseTest()
        {
            TestCodeFix("class X { private X Get() { return base; } }", string.Empty);
        }

        private static void TestCodeFix(string inputCode, string expectedNodeText)
        {
            TestHelper.TestCodeFix<MethodDeclarationSyntax>(new PotentialStaticMethodRefactoring(), inputCode, expectedNodeText);
        }
    }
}
