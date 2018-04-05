using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.LinesOfCode;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class LinesOfCodeRefactoringTesting
    {
        [TestMethod]
        public void EmptyMethodTest()
        {
            MethodDiagnosticTest(0, false);
        }

        [TestMethod]
        public void ShortMethodTest()
        {
            MethodDiagnosticTest(5, false);
        }

        [TestMethod]
        public void LongMethodTest()
        {
            MethodDiagnosticTest(20, true);
        }

        private static string GenerateMethodCode(int linesOfCode)
        {
            var methodCode = new StringBuilder("public void A() {\r\n");

            for (var lineIndex = 0; lineIndex < linesOfCode; ++lineIndex)
            {
                methodCode.Append("int v" + lineIndex + " = 0;\r\n");
            }

            return methodCode + "}";
        }

        private static void MethodDiagnosticTest(int linesOfCode, bool methodToLong)
        {
            var inputCode = GenerateMethodCode(linesOfCode);
            TestHelper.TestMetric<MethodDeclarationSyntax>(new LinesOfCodeRefactoring(), inputCode, methodToLong, null);
        }
    }
}
