using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Helper;
using Refactoring.Refactorings.BooleanConstantSimplifier;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class BooleanConstantSimplifierRefactoringTesting
    {
        [TestMethod]
        public void MixedBooleanConstantExpressionCodeFixTest()
        {
            const string source = "var x = true && false || !true;";
            TestCodeFix(source, "false");
        }

        [TestMethod]
        public void MixedBooleanConstantExpressionDiagnosticTest()
        {
            const string source = "var x = true && false || !true;";
            TestDiagnostic(source, RefactoringMessageFactory.BooleanConstantSimplifierMessage(false));
        }
        
        [TestMethod]
        public void SimpleAndExpressionCodeFixTest()
        {
            const string source = "var y = true && false;";
            TestCodeFix(source, "false");
        }
        
        [TestMethod]
        public void SimpleAndExpressionDiagnosticTest()
        {
            const string source = "var y = true && false;";
            TestDiagnostic(source, RefactoringMessageFactory.BooleanConstantSimplifierMessage(false));
        }
        
        [TestMethod]
        public void SimpleOrExpressionCodeFixTest()
        {
            const string source = "var z = true || false;";
            TestCodeFix(source, "true");
        }

        [TestMethod]
        public void SimpleOrExpressionDiagnosticTest()
        {
            const string source = "var z = true || false;";
            TestDiagnostic(source, RefactoringMessageFactory.BooleanConstantSimplifierMessage(true));
        }
        
        [TestMethod]
        public void SimpleNotExpressionCodeFixTest()
        {
            const string source = "var z = !false;";
            TestCodeFix(source, "true");
        }


        [TestMethod]
        public void SimpleNotExpressionDiagnosticTest()
        {
            const string source = "var z = !false;";
            TestDiagnostic(source, RefactoringMessageFactory.BooleanConstantSimplifierMessage(true));
        }
        
        [TestMethod]
        public void SimpleBracketExpressionCodeFixTest()
        {
            const string source = "var z = !(false || true);";
            TestCodeFix(source, "false");
        }
        
        [TestMethod]
        public void SimpleBracketExpressionDiagnosticTest()
        {
            const string source = "var z = !(false || true);";
            TestDiagnostic(source, RefactoringMessageFactory.BooleanConstantSimplifierMessage(false));
        }

        [TestMethod]
        public void DoubleNotTest()
        {
            const string source = "bool X() { return !!false; }";
            TestCodeFix(source, "false");
        }

        [TestMethod]
        public void BracketDoubleNotTest()
        {
            const string source = "bool X() { return !(!true); }";
            TestCodeFix(source, "true");
        }

        [TestMethod]
        public void UnnecessaryTopLevelBracketTest()
        {
            var source = "bool X() { return (false); }";
            TestCodeFix(source, "false");
            source = "bool X() { return ((true)); }";
            TestCodeFix(source, "true");
        }

        [TestMethod]
        public void MixedBooleanExpressionTest1()
        {
            const string source = "bool X() { return true && ((false) || !true); }";
            TestCodeFix(source, "false");
        }

        [TestMethod]
        public void MixedBooleanExpressionTest2()
        {
            const string source = "bool Y() { return false && (!(true)) || true";
            TestCodeFix(source, "true");
        }

        private static void TestDiagnostic(string inputCode, string outputMessage)
        {
            TestHelper.TestDiagnosticResult<BinaryExpressionSyntax>(new BooleanConstantSimplifierRefactoring(), inputCode, outputMessage, FindNode);
        }

        private static void TestCodeFix(string inputCode, string expectedNodeText)
        {
            TestHelper.TestCodeFix<BinaryExpressionSyntax>(new BooleanConstantSimplifierRefactoring(), inputCode, expectedNodeText, FindNode);
        }
        
        private static SyntaxNode FindNode(SyntaxNode node)
        {
            if (node is BinaryExpressionSyntax || node is PrefixUnaryExpressionSyntax || node is ParenthesizedExpressionSyntax)
                return node;

            return node.ChildNodes().Select(FindNode).FirstOrDefault(foundNode => foundNode != null);
        }
    }
}