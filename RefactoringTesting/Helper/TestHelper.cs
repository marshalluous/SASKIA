using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring;
using System;
using System.Linq;

namespace RefactoringTesting.Helper
{
    internal static class TestHelper
    {
        public static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source).GetRoot();
        }
        
        public static void TestDiagnosticResult<T>(IRefactoring refactoring, string inputCode, string outputMessage, Func<SyntaxNode, SyntaxNode> findNodeFunc)
        {
            var node = Compile(inputCode);
            node = findNodeFunc(node);
            Assert.IsNotNull(node);
            var diagnosticResult = refactoring.DoDiagnosis(node);

            if (!diagnosticResult.DiagnosticFound)
            {
                Assert.AreEqual(string.Empty, outputMessage);
                return;
            }
            
            Assert.AreEqual(outputMessage, diagnosticResult.Message);
        }

        public static void TestDiagnosticResult<T>(IRefactoring refactoring, string inputCode, string outputMessage = "")
        {
            TestDiagnosticResult<T>(refactoring, inputCode, outputMessage, FindNodeOfType<T>);
        }

        public static void TestCodeFix<T>(IRefactoring refactoring, string inputCode, string expectedNodeText, Func<SyntaxNode, SyntaxNode> findNodeFunc)
        {
            var node = Compile(inputCode);
            node = findNodeFunc(node);
            Assert.IsNotNull(node);
            var resultNodes = refactoring.GetFixableNodes(node);

            if (resultNodes == null)
            {
                Assert.AreEqual(string.Empty, expectedNodeText);
                return;
            }

            var resultNode = refactoring.GetFixableNodes(node).First();
            Assert.AreEqual(expectedNodeText, resultNode.ToString());
        }
        
        public static void TestCodeFix<T>(IRefactoring refactoring, string inputCode, string expectedNodeText)
        {
            TestCodeFix<T>(refactoring, inputCode, expectedNodeText, FindNodeOfType<T>);
        }

        public static void TestMetric<T>(IRefactoring refactoring, string inputCode, bool diagnosticFound, object metricValue, Func<SyntaxNode, SyntaxNode> findNodeFunc)
        {
            var node = Compile(inputCode);
            node = findNodeFunc(node);
            Assert.IsNotNull(node);
            var diagnosticInfo = refactoring.DoDiagnosis(node);
            Assert.AreEqual(diagnosticFound, diagnosticInfo.DiagnosticFound);
            Assert.AreEqual(metricValue, diagnosticInfo.AdditionalInformation);
        }

        public static void TestMetric<T>(IRefactoring refactoring, string inputCode, bool diagnosticFound, object metricValue)
        {
            TestMetric<T>(refactoring, inputCode, diagnosticFound, metricValue, FindNodeOfType<T>);
        }

        public static SyntaxNode FindNodeOfType<T>(SyntaxNode node)
        {
            if (node is T)
                return node;

            return node.ChildNodes().Select(FindNodeOfType<T>)
                .FirstOrDefault(foundNode => foundNode != null);
        }
    }
}