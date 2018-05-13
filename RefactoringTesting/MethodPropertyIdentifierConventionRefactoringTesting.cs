using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.MethodPropertyIdentifierConvention;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class MethodPropertyIdentifierConventionRefactoringTesting
    {
        [TestMethod]
        public void PropertyLowerCaseNameTest()
        {
            TestPropertyNaming("fishHans", "FishHans");
            TestPropertyNaming("ageOfFish", "AgeOfFish");
        }

        [TestMethod]
        public void PropertyUnderlineNameTest()
        {
            TestPropertyNaming("james_bond", "JamesBond");
            TestPropertyNaming("_joel_egger", "JoelEgger");
        }

        [TestMethod]
        public void CorrectPropertyNameTest()
        {
            TestPropertyNaming("SystemOfADown", string.Empty);
            TestPropertyNaming("AugustBurnsRed", string.Empty);
        }

        [TestMethod]
        public void MethodLowerCaseNameTest()
        {
            TestMethodNaming("fishHans", "FishHans");
            TestMethodNaming("ageOfFish", "AgeOfFish");
        }
        
        [TestMethod]
        public void MethodUnderlineNameTest()
        {
            TestMethodNaming("james_bond", "JamesBond");
            TestMethodNaming("_joel_egger", "JoelEgger");
        }

        [TestMethod]
        public void MethodPropertyNameTest()
        {
            TestMethodNaming("SystemOfADown", string.Empty);
            TestMethodNaming("AugustBurnsRed", string.Empty);
        }

        [TestMethod]
        public void ExternMethodNameTest()
        {
            const string inputCode = "public class X { public static extern void Y(); }";
            TestHelper.TestCodeFix<MethodDeclarationSyntax>(new MethodPropertyIdentifierConventionRefactoring(), inputCode,
                string.Empty);
        }

        private static void TestMethodNaming(string methodName, string expectedMethodName)
        {
            var inputCode = "public class X { public void " + methodName + "() {} }";
            var expectedOutputCode = "public void " + expectedMethodName + "() {}";

            if (string.IsNullOrEmpty(expectedMethodName))
                expectedOutputCode = string.Empty;

            TestHelper.TestCodeFix<MethodDeclarationSyntax>(new MethodPropertyIdentifierConventionRefactoring(), inputCode,
                expectedOutputCode);
        }

        private static void TestPropertyNaming(string propertyName, string expectedPropertyName)
        {
            var inputCode = "public class X { public int " + propertyName + " { get; } }";
            var expectedOutputCode = "public int " + expectedPropertyName + "{ get; }";

            if (string.IsNullOrEmpty(expectedPropertyName))
                expectedOutputCode = string.Empty;

            TestHelper.TestCodeFix<PropertyDeclarationSyntax>(new MethodPropertyIdentifierConventionRefactoring(), inputCode,
                expectedOutputCode);
        }
    }
}
