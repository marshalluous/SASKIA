using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Helper;
using Refactoring.Refactorings.ConditionalComplexity;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class ConditionalComplexityRefactoringTesting
    {
        [TestMethod]
        public void EmptyMethodTest()
        {
            TestComplexity(string.Empty, false, 1);
        }

        [TestMethod]
        public void NonConditionalComplexityMethodTest()
        {
            TestComplexity("bool x = true; if (x) { x = false; } else { x = true; }", false, 2);
        }

        [TestMethod]
        public void ConditionalComplexityIfMethodTest()
        {
            TestComplexity("int a = 0; bool x = true; if (x && x) { return x ? x : !x; } if(x) { a = 5; }" +
                                         "if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; }" +
                                         "if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; }", true, 13);
        }

        [TestMethod]
        public void ConditionalComplexityIfMethodDiagnosticTest()
        {
            TestDiagnostic("int a = 0; bool x = true; if (x && x) { return x ? x : !x; } if(x) { a = 5; }" +
                                         "if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; }" +
                                         "if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; }",
                                         RefactoringMessageFactory.ConditionalComplexityMessage(13));
        }

        [TestMethod]
        public void WhileLoopTest()
        {
            TestComplexity("int x = 4; while (x < 5) { ++x; }", false, 2);
        }

        [TestMethod]
        public void ForLoopTest()
        {
            TestComplexity("for (int x=0; x<10; ++x) {} for (int y=0; y<15; ++y) {}", false, 3);
        }

        [TestMethod]
        public void AndOrTest()
        {
            TestComplexity("bool x = 4 < 5 && true || false;", false, 3);
        }

        [TestMethod]
        public void SwitchCaseTest()
        {
            TestComplexity("int a = 12; switch (a) { case 1: break; case 2: break; case 3: break; }", false, 4);
        }

        [TestMethod]
        public void TryCatchTest()
        {
            TestComplexity("try { int x = 4; } catch (Exception ex) { }", false, 2);
        }

        [TestMethod]
        public void CatchFilterClauseTest()
        {
            TestComplexity("try { int z = 12; } catch (Exception ex) when ex is Exception {}", false, 3);
        }

        [TestMethod]
        public void ConditionalAccessCodeFixTest()
        {
            TestComplexity("string s = null; int? length = s?.Length;", false, 2);
        }


        [TestMethod]
        public void ConditionalAccessDiagnosticTest()
        {
            TestDiagnostic("string s = null; int? length = s?.Length;", string.Empty);
        }

        [TestMethod]
        public void ConditionalExpressionTest()
        {
            TestComplexity("int x = 12; string s = x == 12 ? \"12\" : null;", false, 2);
        }

        [TestMethod]
        public void ElseIfTest()
        {
            TestComplexity("int x = 12; if (x == 11) { } else if (x == 34) { } else {}", false, 3);
        }

        [TestMethod]
        public void ForEachTest()
        {
            TestComplexity("string[] sa; foreach (string s : sa) { }", false, 2);
        }

        [TestMethod]
        public void ForTest()
        {
            TestComplexity("for (var i = 0; i < 10; ++i) {}", false, 2);
        }

        [TestMethod]
        public void IfTest()
        {
            TestComplexity("int x = 4; if (x == 12) {}", false, 2);
        }

        [TestMethod]
        public void WhileTest()
        {
            TestComplexity("while (true) {} while (false) {}", false, 3);
        }

        [TestMethod]
        public void MixedTest1()
        {
            TestComplexity("bool x = false; if (x && x ? x : false) { while (true && false) {} }", false, 6);
        }

        [TestMethod]
        public void MixedTest2()
        {
            TestComplexity("try { int x = 12; if (x > 5 && x < 20) { } else {} } catch (Exception e) when e is Exception {}", false, 5);
        }

        private static string CreateMethodCode(string code)
        {
            return "public void X() {" + code + "}";
        }
        
        private static void TestDiagnostic(string inputCode, string outputMessage)
        {
            var methodCode = CreateMethodCode(inputCode);
            TestHelper.TestDiagnosticResult<MethodDeclarationSyntax>(new ConditionalComplexityRefactoring(), methodCode, outputMessage);
        }

        private static void TestComplexity(string inputCode, bool diagnosticFound, int metricValue)
        {
            var methodCode = CreateMethodCode(inputCode);
            TestHelper.TestMetric<MethodDeclarationSyntax>(new ConditionalComplexityRefactoring(), methodCode, diagnosticFound, metricValue);
        }
    }
}