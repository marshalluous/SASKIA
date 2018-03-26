using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.NotOperatorInversion;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class NotOperationInversionRefactoringTesting
    {
        [TestMethod]
        public void NotEqualsEqualsTest()
        {
            TestCodeFix("var x = !(4 == 12);", "4 != 12");
        }

        [TestMethod]
        public void NestedNotExpressionTest()
        {
            TestCodeFix("var x = !(!(4 != 12))", "4 != 12");
        }

        [TestMethod]
        public void NotLessThanExpressionTest()
        {
            TestCodeFix("var x = !(4 < 12)", "4 >= 12");
        }

        [TestMethod]
        public void NotGreaterThanExpressionTest()
        {
            TestCodeFix("var x = !(4 > 12)", "4 <= 12");
        }

        [TestMethod]
        public void NotLessThanEqualsTest()
        {
            TestCodeFix("var x = !(4 <= 12)", "4 > 12");
        }

        [TestMethod]
        public void NotGreaterThanEqualsTest()
        {
            TestCodeFix("var x = !(4 >= 12)", "4 < 12");
        }
        
        private static void TestCodeFix(string input, string expectedOutput)
        {
            var node = Compile(input);
            node = FindNode(node);
            Assert.IsNotNull(node);
            var refactoring = new NotOperatorInversionRefactoring();
            var resultNode = refactoring.ApplyFix(node).First();
            Assert.AreEqual(expectedOutput, resultNode.ToString());
        }

        private static SyntaxNode FindNode(SyntaxNode node)
        {
            return node is PrefixUnaryExpressionSyntax ?
                node :
                node.ChildNodes().Select(FindNode).FirstOrDefault(foundNode => foundNode != null);
        }

        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
    }
}
