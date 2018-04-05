using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.IfAndElseBlockEquals;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class IfAndElseBlockEqualsRefactoringTesting
    {
        [TestMethod]
        public void IfAndElseBlockDifferentTest()
        {
            const string source = "if (true) { int a = 12; } else { int b = 13; }";
            TestCodeFix(source, string.Empty, true);
        }

        [TestMethod]
        public void NoElseBlockTest()
        {
            const string source = "if (true) { int a = 32; }";
            TestCodeFix(source, string.Empty, true);
        }

        [TestMethod]
        public void IfAndElseBlockEmptyTest()
        {
            const string source = "if (true) { } else { }";
            TestCodeFix(source, new string[0]);
        }

        [TestMethod]
        public void IfAndElseBlockSameCodeTest()
        {
            const string source = "if (true) { int x = 12; } else { int x = 12; }";
            TestCodeFix(source, new [] { "int x = 12;" });
        }

        [TestMethod]
        public void IfAndElseBlockSameCodeButIfBlockWithCurlyBrackets()
        {
            const string source = "if (true) { int x = 12; } else int x = 12;";
            TestCodeFix(source, new [] { "int x = 12;" });
        }

        [TestMethod]
        public void IgnoreElseIfBlocksTest()
        {
            const string source = "if (true) { int x = 12; } else if (4 == 12) { int x = 12; }";
            TestCodeFix(source, string.Empty, true);
        }

        [TestMethod]
        public void IgnoreElseIfBlocksWithElseTest()
        {
            const string source = "if (true) { int x = 12; } else if (4 == 12) { int x = 12; } else { int x = 12; }";
            TestCodeFix(source, string.Empty, true);
        }

        [TestMethod]
        public void IfAndElseBlockTheSameCodeWithMultipleLines()
        {
            const string source =
                "if (true) { int x = 12;\r\nint y = 14;\r\n } else  { int x = 12;\r\nint y = 14;\r\n }";
            TestCodeFix(source, new [] { "int x = 12;", "int y = 14;" });
        }

        private static void TestCodeFix(string inputCode, string expectedOutputCode, bool expectNoRefactoring = false)
        {
            inputCode = "void A() { " + inputCode + " }";
            var node = TestHelper.Compile(inputCode);
            var refactoring = new IfAndElseBlockEqualsRefactoring();
            node = TestHelper.FindNodeOfType<IfStatementSyntax>(node);
            Assert.IsNotNull(node);
            var resultNodes = refactoring.GetFixableNodes(node);

            if (expectNoRefactoring)
            {
                Assert.IsNull(resultNodes);
                return;
            }

            Assert.AreEqual(expectedOutputCode, resultNodes.First().GetText().ToString().Trim());
        }

        private static void TestCodeFix(string inputCode, IEnumerable<string> expectedStatements)
        {
            inputCode = "void A() { " + inputCode + " }";
            var node = TestHelper.Compile(inputCode);
            var refactoring = new IfAndElseBlockEqualsRefactoring();
            node = TestHelper.FindNodeOfType<IfStatementSyntax>(node);
            Assert.IsNotNull(node);
            var resultNodes = refactoring.GetFixableNodes(node);
            CompareStringLists(expectedStatements.ToList(), resultNodes.Select(resultNode => resultNode.GetText().ToString().Trim()).ToList());
        }

        private static void CompareStringLists(IList<string> first, IList<string> second)
        {
            Assert.AreEqual(first.Count, second.Count);

            for (var index = 0; index < first.Count; ++index)
            {
                Assert.AreEqual(first[index], second[index]);
            }
        }
    }
}
