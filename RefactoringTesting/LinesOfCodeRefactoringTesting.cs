using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.LinesOfCode;
using RefactoringTesting.Helper;
using System.Text;

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

        [TestMethod]
        public void ReallyLongMethodTest()
        {
            MethodDiagnosticTest(45, true);
        }
        
        private static string GenerateMethodCode(int linesOfCode, int linesOfDocumentCode)
        {
            var methodCode = new StringBuilder();
            CreateMethodDocumentCode(linesOfDocumentCode, methodCode);
            methodCode.Append("public void X() {");
            CreateMethodBodyCode(linesOfCode, methodCode);
            return methodCode + "}";
        }

        private static void CreateMethodBodyCode(int linesOfCode, StringBuilder methodCode)
        {
            for (var lineIndex = 0; lineIndex < linesOfCode; ++lineIndex)
            {
                methodCode.Append("int v" + lineIndex + " = 0;\r\n");
            }
        }

        private static void CreateMethodDocumentCode(int linesOfDocumentCode, StringBuilder methodCode)
        {
            if (linesOfDocumentCode <= 0)
                return;

            for (var lineIndex = 0; lineIndex < linesOfDocumentCode; ++lineIndex)
            {
                methodCode.Append("/// <summary>");
            }
        }

        private static void MethodDiagnosticTest(int linesOfCode, bool methodToLong, int linesOfDocumentCode = 0)
        {
            var inputCode = GenerateMethodCode(linesOfCode, linesOfDocumentCode);
            TestHelper.TestMetric<MethodDeclarationSyntax>(new LinesOfCodeRefactoring(), inputCode, methodToLong, null);
        }
    }
}