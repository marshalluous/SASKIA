﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.UseOfVar;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class UseOfVarRefactoringTesting
    {
        [TestMethod]
        public void SimpleLocalVariableWithoutAssignmentTest()
        {
            TestCodeFix("int x;", string.Empty);
        }

        [TestMethod]
        public void SimpleLocalVariableWithAssignmentTest()
        {
            TestCodeFix("int x = 4;", "var x = 4;");
            TestCodeFix("var x = 4;", string.Empty);
        }

        [TestMethod]
        public void ArrayLocalVariableWithoutAssignmentTest()
        {
            TestCodeFix("int[] arr;", string.Empty);
        }

        [TestMethod]
        public void ArrayLocalVariableWithAssignmentTest()
        {
            TestCodeFix("int[] arr = new int[]{45};", "var arr = new int[]{45};");
        }

        /*
        [TestMethod]
        public void NotMatchingPrimitiveTypeAssignmentTest()
        {
            TestCodeFix("int x = 3.2;", string.Empty);
        }
        */

        private static void TestCodeFix(string declaration, string refactoredDeclaration)
        {
            var source = "class X { void Y() { " + declaration + " } }";
            TestHelper.TestCodeFix<LocalDeclarationStatementSyntax>
                (new UseOfVarRefactoring(), source, refactoredDeclaration);
        }
    }
}
