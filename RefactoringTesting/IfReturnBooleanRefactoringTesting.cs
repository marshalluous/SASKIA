using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.IfReturnBoolean;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class IfReturnBooleanRefactoringTesting
    {
        [TestMethod]
        public void TestReturnBoolean()
        {
            var source = GetCodeTemplate("if (true == false) { return true; } else { return false; }");
            TestCodeFix(source, "return true == false;");
        }

        [TestMethod]
        public void TestReturnFalseReturnTrueSample()
        {
            var source = GetCodeTemplate("if (true == false) { return false; } else { return true; }");
            TestCodeFix(source, "return !(true == false);");
        }

        [TestMethod]
        public void TestReturnInBrackets()
        {
            var source = GetCodeTemplate("if (A()) { { return true; } } else {{return false; }}");
            TestCodeFix(source, "return A();");
            source = GetCodeTemplate("if (B()) { { return false; }} else {{  return true; }}");
            TestCodeFix(source, "return !B();");
        }

        [TestMethod]
        public void TestReturnLiteralInBrackets()
        {
            var source = GetCodeTemplate("if (A() && B()) { return ((true)); } else { return (((false))); }");
            TestCodeFix(source, "return A() && B();");
            source = GetCodeTemplate("if (A() || B()) { return (false); } else { return ( ( true)); }");
            TestCodeFix(source, "return !(A() || B());");
        }

        [TestMethod]
        public void TestBracketBlocksAndBlacketLiteral()
        {
            var source =
                GetCodeTemplate("if (expr is DateTime) {{ return ((((false)))); }} else {{{{ return ((true)); }}}}");
            TestCodeFix(source, "return !(expr is DateTime);");
        }

        [TestMethod]
        public void TestMethodCallCondition()
        {
            var source = GetCodeTemplate("if (A()) { return false; } else { return true; }");
            TestCodeFix(source, "return !A();");

            source = GetCodeTemplate("int x = 4; if (nameof(x) == \"x\") { return false; } else {return true; }");
            TestCodeFix(source, "return !(nameof(x) == \"x\");");
        }

        [TestMethod]
        public void TestIntegerComparisonCondition()
        {
            var source = GetCodeTemplate("if (4 < 12) { return false; } else return true;");
            TestCodeFix(source, "return !(4 < 12);");
        }

        private static string GetCodeTemplate(string code)
        {
            return "class X { public bool Y() {" + code + "}}";
        }

        private static void TestCodeFix(string inputCode, string expectedNodeText)
        {
            TestHelper.TestCodeFix<IfStatementSyntax>(new IfReturnBooleanRefactoring(), inputCode, expectedNodeText);
        }
    }
}