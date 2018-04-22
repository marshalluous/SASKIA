using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.IllegalFieldAccess;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class IllegalFieldAccessRefactoringTesting
    {
        [TestMethod]
        public void PublicFieldTest()
        {
            TestCodeFix("public class X { public int a; }", "private int a;");
        }

        [TestMethod]
        public void ProtectedFieldTest()
        {
            TestCodeFix("class Y { protected float b; }", "private float b;");
        }

        [TestMethod]
        public void InternalFieldTest()
        {
            TestCodeFix("class Z { internal string z; }", "private string z;");
        }

        [TestMethod]
        public void ProtectedInternalFieldTest()
        {
            TestCodeFix("class A { protected internal bool a; }", "private bool a;");
        }

        [TestMethod]
        public void UnspecifiedVisibilityFieldTest()
        {
            TestCodeFix("class N { double x; }", "private double x;");
        }

        [TestMethod]
        public void StaticPublicFieldTest()
        {
            TestCodeFix("public class X { public static int a; }", "private static int a;");
        }

        [TestMethod]
        public void StaticProtectedFieldTest()
        {
            TestCodeFix("class Y { protected static float b; }", "private static float b;");
        }

        [TestMethod]
        public void StaticInternalFieldTest()
        {
            TestCodeFix("class Z { internal static string z; }", "private static string z;");
        }

        [TestMethod]
        public void StaticProtectedInternalFieldTest()
        {
            TestCodeFix("class A { protected static internal bool a; }", "private static bool a;");
        }

        [TestMethod]
        public void StaticUnspecifiedVisibilityFieldTest()
        {
            TestCodeFix("class N { static double x; }", "private static double x;");
        }

        [TestMethod]
        public void TestCodeFix(string inputCode, string expectedNodeText)
        {
            TestHelper.TestCodeFix<FieldDeclarationSyntax>(new IllegalFieldAccessRefactoring(), inputCode, expectedNodeText);
        }
    }
}
