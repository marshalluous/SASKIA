using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.LinesOfCode;

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
            var node = Compile(GenerateMethodCode(linesOfCode));
            var refactoring = new LinesOfCodeRefactoring();
            var diagnosticResult = refactoring.DoDiagnosis((MethodDeclarationSyntax) node.ChildNodes().First());
            Assert.AreEqual(methodToLong, diagnosticResult.DiagnosticFound);
        }
        
        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
    }
}
