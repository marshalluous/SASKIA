using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.ConditionalComplexity;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class ConditionalComplexityRefactoringTesting
    {
        [TestMethod]
        public void EmptyMethodTest()
        {
            TestForConditionalComplexity(string.Empty, false, 1);
        }

        [TestMethod]
        public void NonConditionalComplexityMethodTest()
        {
            TestForConditionalComplexity("bool x = true; if (x) { x = false; } else { x = true; }", false, 2);
        }

        [TestMethod]
        public void ConditionalComplexityIfMethodTest()
        {
            TestForConditionalComplexity("int a = 0; bool x = true; if (x && x) { return x ? x : !x; } if(x) { a = 5; }" +
                                         "if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; }" +
                                         "if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; } if (x) { a = 5; }", true, 13);
        }

        [TestMethod]
        public void WhileLoopTest()
        {
            TestForConditionalComplexity("int x = 4; while (x < 5) { ++x; }", false, 2);
        }

        [TestMethod]
        public void ForLoopTest()
        {
            TestForConditionalComplexity("for (int x=0; x<10; ++x) {} for (int y=0; y<15; ++y) {}", false, 3);
        }

        [TestMethod]
        public void AndOrTest()
        {
            TestForConditionalComplexity("bool x = 4 < 5 && true || false;", false, 3);
        }

        [TestMethod]
        public void SwitchCaseTest()
        {
            TestForConditionalComplexity("int a = 12; switch (a) { case 1: break; case 2: break; case 3: break; }", false, 4);
        }

        [TestMethod]
        public void TryCatchTest()
        {
            TestForConditionalComplexity("try { int x = 4; } catch (Exception ex) { }", false, 2);
        }

        [TestMethod]
        public void CatchFilterClauseTest()
        {
            TestForConditionalComplexity("try { int z = 12; } catch (Exception ex) when ex is Exception {}", false, 3);
        }

        [TestMethod]
        public void ConditionalAccessTest()
        {
            TestForConditionalComplexity("string s = null; int? length = s?.Length;", false, 2);
        }

        [TestMethod]
        public void ConditionalExpressionTest()
        {
            TestForConditionalComplexity("int x = 12; string s = x == 12 ? \"12\" : null;", false, 2);
        }

        [TestMethod]
        public void ElseIfTest()
        {
            TestForConditionalComplexity("int x = 12; if (x == 11) { } else if (x == 34) { } else {}", false, 3);
        }

        [TestMethod]
        public void ForEachTest()
        {
            TestForConditionalComplexity("string[] sa; foreach (string s : sa) { }", false, 2);
        }

        [TestMethod]
        public void ForTest()
        {
            TestForConditionalComplexity("for (var i = 0; i < 10; ++i) {}", false, 2);
        }

        [TestMethod]
        public void IfTest()
        {
            TestForConditionalComplexity("int x = 4; if (x == 12) {}", false, 2);
        }

        [TestMethod]
        public void WhileTest()
        {
            TestForConditionalComplexity("while (true) {} while (false) {}", false, 3);
        }

        [TestMethod]
        public void MixedTest1()
        {
            TestForConditionalComplexity("bool x = false; if (x && x ? x : false) { while (true && false) {} }", false, 6);
        }

        [TestMethod]
        public void MixedTest2()
        {
            TestForConditionalComplexity("try { int x = 12; if (x > 5 && x < 20) { } else {} } catch (Exception e) when e is Exception {}", false, 5);
        }

        private static string CreateMethodCode(string code)
        {
            return "public void X() {" + code + "}";
        }

        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
        
        private static void TestForConditionalComplexity(string inputCode, bool hasConditionalComplexity, int mcCabeCount)
        {
            var node = Compile(CreateMethodCode(inputCode));
            var methodNode = node.ChildNodes().First();
            var refactoring = new ConditionalComplexityRefactoring();
            Assert.IsNotNull(node);
            var diagnosticInfo = refactoring.DoDiagnosis(methodNode);
            Assert.AreEqual(hasConditionalComplexity, diagnosticInfo.DiagnosticFound);
            Assert.AreEqual(mcCabeCount, diagnosticInfo.AdditionalInformation);
        }
    }
}