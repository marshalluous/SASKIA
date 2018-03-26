using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class PureExpressionCheckerTesting
    {
        [TestMethod]
        public void TestMathExpressions()
        {
            CheckPureness("4 + 34", true);
            CheckPureness("12 - 45", true);
            CheckPureness("5 * 12", true);
        }

        [TestMethod]
        public void TestUnaryExpressions()
        {
            CheckPureness("!b", true);
            CheckPureness("+(-(x))", true);
        }

        [TestMethod]
        public void TestMethodCall()
        {
            CheckPureness("x == A();", false);
        }

        [TestMethod]
        public void TestAssignment()
        {
            CheckPureness("x = false;", false);
        }

        [TestMethod]
        public void TestIncrementDecrement()
        {
            CheckPureness("x = ++x;", false);
            CheckPureness("x = x--;", false);
        }
        
        private static void CheckPureness(string source, bool expectIsPure)
        {
            var node = FindNode(Compile("var x = (" + source + ");"));
            var visitor = new PureExpressionCheckerVisitor(SyntaxNodeHelper.FindAncestorOfType<CompilationUnitSyntax>(node.GetFirstToken()));
            Assert.IsNotNull(node);
            var actualIsPure = visitor.Visit(node);
            Assert.AreEqual(expectIsPure, actualIsPure);
        }
        
        private static SyntaxNode FindNode(SyntaxNode node)
        {
            if (node is ParenthesizedExpressionSyntax)
            {
                return node;
            }

            return node.ChildNodes().Select(FindNode)
                .FirstOrDefault(foundNode => foundNode != null);
        }

        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
    }
}
