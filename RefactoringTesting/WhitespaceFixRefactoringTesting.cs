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

        private static void TestCodeFix(string inputCode, string outputCode)
        {
            TestHelper.TestCodeFix<NamespaceDeclarationSyntax>(new WhitespaceFixRefactoring(), inputCode, outputCode);
        }
    }
}
