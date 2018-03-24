﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Refactoring.Refactorings.BooleanConstantComparison;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class BooleanConstantComparisonRefactoringTesting
    {
        [TestMethod]
        public void CompareWithTrueLiteralOnRightHandSideTest()
        {
            TestCodeFix<BinaryExpressionSyntax>("var b = x == true;", "x");
        }

        [TestMethod]
        public void CompareWithFalseLiteralOnRightHandSideTest()
        {
            TestCodeFix<BinaryExpressionSyntax>("var b = x == false;", "!x");
        }

        [TestMethod]
        public void EqualsEqualsNestedInEqualsEqualsTest()
        {
            TestCodeFix<BinaryExpressionSyntax>("var x = 4 == 12 == true;", "4 == 12");
        }

        [TestMethod]
        public void CompareWithTrueLiteralOnLeftHandSide()
        {
            TestCodeFix<BinaryExpressionSyntax>("var b = true == x", "x");
        }

        [TestMethod]
        public void CompareWithFalseLiteralOnLeftHandSide()
        {
            TestCodeFix<BinaryExpressionSyntax>("var b = false == x", "!x");
        }

        [TestMethod]

        public void NotEqualsCompareWithTrueLiteralOnRightHandSideTest()
        {
            TestCodeFix<BinaryExpressionSyntax>("var b = x != true;", "!x");
        }

        [TestMethod]
        public void NotEqualsCompareWithFalseLiteralOnRightHandSideTest()
        {
            TestCodeFix<BinaryExpressionSyntax>("var b = x != false;", "x");
        }

        [TestMethod]
        public void NotEqualsCompareWithTrueLiteralOnLeftHandSide()
        {
            TestCodeFix<BinaryExpressionSyntax>("var b = true != x", "!x");
        }

        [TestMethod]
        public void NotEqualsCompareWithFalseLiteralOnLeftHandSide()
        {
            TestCodeFix<BinaryExpressionSyntax>("var b = false != x", "x");
        }

        [TestMethod]
        public void MethodCallEqualsEqualsComparison()
        {
            TestCodeFix<BinaryExpressionSyntax>("var k = true == A();", "A()");
            TestCodeFix<BinaryExpressionSyntax>("var k = A() == false;", "!A()");
        }

        [TestMethod]
        public void MethodCallNotEqualsComparison()
        {
            TestCodeFix<BinaryExpressionSyntax>("var k = false != A();", "A()");
            TestCodeFix<BinaryExpressionSyntax>("var k = A() != true;", "!A()");
        }
        
        [TestMethod]
        public void ComplexExpressionEqualsEqualsTrue()
        {
            TestCodeFix<BinaryExpressionSyntax>("var k = 4 < 5 == true;", "4 < 5");
        }

        [TestMethod]
        public void ComplexExpressionEqualsEqualsFalse()
        {
            TestCodeFix<BinaryExpressionSyntax>("var k = 4 < 12 == false;", "!(4 < 12)");
        }

        [TestMethod]
        public void TestSpecialNameofComparison()
        {
            TestCodeFix<BinaryExpressionSyntax>("var x = ((nameof(x) == \"x\") == false);", "!(nameof(x) == \"x\")");
        }

        private static void TestCodeFix<T>(string inputCode, string expectedNodeText)
        {
            var node = Compile(inputCode);
            var refactoring = new BooleanConstantComparisonRefactoring();
            node = FindNodeOfType<T>(node);
            Assert.IsNotNull(node);
            var resultNode = refactoring.ApplyFix(node).First();
            Assert.AreEqual(expectedNodeText, resultNode.ToString());
        }

        private static SyntaxNode FindNodeOfType<T>(SyntaxNode node)
        {
            if (node is T)
            {
                return node;
            }

            return node.ChildNodes().Select(FindNodeOfType<T>)
                .FirstOrDefault(foundNode => foundNode != null);
        }
        
        private static SyntaxNode Compile(string source)
        {
            return CSharpSyntaxTree.ParseText(source)
                .GetRoot();
        }
    }
}