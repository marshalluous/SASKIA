using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.WhitespaceFix;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class WhitespaceFixRefactoringTesting
    {
        [TestMethod]
        public void TestWhitespaceFix1()
        {
            TestCodeFix("namespace X     {     class N {   \r\t } }", "namespace X\r\n{\r\n    class N\r\n    {\r\n    }\r\n}");
        }

        [TestMethod]
        public void TestWhitespaceFix2()
        {
            TestCodeFix("namespace Y    {       \t delegate void X(); }", "namespace Y\r\n{\r\n    delegate void X();\r\n}");
        }

        [TestMethod]
        public void TestWhitespaceFix3()
        {
            TestCodeFix("namespace Z{ public interface A { void N(); } }",
                "namespace Z\r\n{\r\n    public interface A\r\n    {\r\n        void N();\r\n    }\r\n}");
        }

        [TestMethod]
        public void TestWhitespaceFix4()
        {
            TestCodeFix("namespace X{class \rY{void \n Z() { if (4 < 12) { }}\t}}}",
                "namespace X\r\n{\r\n    class Y\r\n    {\r\n        void Z()\r\n        {\r\n            if (4 < 12)\r\n" +
                "            {\r\n            }\r\n        }\r\n    }\r\n}");
        }

        private static void TestCodeFix(string inputCode, string outputCode)
        {
            TestHelper.TestCodeFix<NamespaceDeclarationSyntax>(new WhitespaceFixRefactoring(), inputCode, outputCode);
        }
    }
}
