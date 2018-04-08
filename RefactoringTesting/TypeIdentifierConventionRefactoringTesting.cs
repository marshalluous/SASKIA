using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.Refactorings.TypeIdentifierConvention;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
    public sealed class TypeIdentifierConventionRefactoringTesting
    {
        [TestMethod]
        public void LowerCaseClassNameTest()
        {
            TestClassNameFix("fish", "Fish");
            TestClassNameFix("bird", "Bird");
            TestClassNameFix("hansPeter", "HansPeter");
        }

        [TestMethod]
        public void UnderlineClassNameTest()
        {
            TestClassNameFix("Fish_builder", "FishBuilder");
            TestClassNameFix("Bird_builder_Destroyer", "BirdBuilderDestroyer");
        }

        [TestMethod]
        public void CorrectClassName()
        {
            TestClassNameFix("FishCreator", "FishCreator");
            TestClassNameFix("MarcelStocker", "MarcelStocker");
        }

        [TestMethod]
        public void LowerCaseDelegateNameTest()
        {
            TestDelegateNameFix("fish", "Fish");
            TestDelegateNameFix("bird", "Bird");
            TestDelegateNameFix("hansPeter", "HansPeter");
        }

        [TestMethod]
        public void UnderlineDelegateNameTest()
        {
            TestDelegateNameFix("Fish_builder", "FishBuilder");
            TestDelegateNameFix("Bird_builder_Destroyer", "BirdBuilderDestroyer");
        }

        [TestMethod]
        public void CorrectDelegateName()
        {
            TestDelegateNameFix("FishCreator", "FishCreator");
            TestDelegateNameFix("MarcelStocker", "MarcelStocker");
        }

        [TestMethod]
        public void LowerCaseEnumNameTest()
        {
            TestEnumNameFix("fish", "Fish");
            TestEnumNameFix("bird", "Bird");
            TestEnumNameFix("hansPeter", "HansPeter");
        }

        [TestMethod]
        public void UnderlineEnumNameTest()
        {
            TestEnumNameFix("Fish_builder", "FishBuilder");
            TestEnumNameFix("Bird_builder_Destroyer", "BirdBuilderDestroyer");
        }

        [TestMethod]
        public void CorrectEnumName()
        {
            TestEnumNameFix("FishCreator", "FishCreator");
            TestEnumNameFix("MarcelStocker", "MarcelStocker");
        }
        
        [TestMethod]
        public void LowerCaseStructNameTest()
        {
            TestStructNameFix("fish", "Fish");
            TestStructNameFix("bird", "Bird");
            TestStructNameFix("hansPeter", "HansPeter");
        }

        [TestMethod]
        public void UnderlineStructNameTest()
        {
            TestStructNameFix("Fish_builder", "FishBuilder");
            TestStructNameFix("Bird_builder_Destroyer", "BirdBuilderDestroyer");
        }

        [TestMethod]
        public void CorrectStructName()
        {
            TestStructNameFix("FishCreator", "FishCreator");
            TestStructNameFix("MarcelStocker", "MarcelStocker");
        }
        
        [TestMethod]
        public void LowerCaseInterfaceNameTest()
        {
            TestInterfaceNameFix("fish", "IFish");
            TestInterfaceNameFix("bird", "IBird");
            TestInterfaceNameFix("hansPeter", "IHansPeter");
        }

        [TestMethod]
        public void UnderlineInterfaceNameTest()
        {
            TestInterfaceNameFix("Fish_builder", "IFishBuilder");
            TestInterfaceNameFix("Bird_builder_Destroyer", "IBirdBuilderDestroyer");
        }

        [TestMethod]
        public void CorrectInterfaceName()
        {
            TestInterfaceNameFix("IFishCreator", "IFishCreator");
            TestInterfaceNameFix("IMarcelStocker", "IMarcelStocker");
        }

        private static void TestEnumNameFix(string enumName, string expectedEnumName)
        {
            TestTypeFix<EnumDeclarationSyntax>("enum", enumName, expectedEnumName);
        }

        private static void TestInterfaceNameFix(string interfaceName, string expectedInterfaceName)
        {
            TestTypeFix<InterfaceDeclarationSyntax>("interface", interfaceName, expectedInterfaceName);
        }

        private static void TestClassNameFix(string className, string expectedClassName)
        {
            TestTypeFix<ClassDeclarationSyntax>("class", className, expectedClassName);
        }

        private static void TestStructNameFix(string structName, string expectedStructName)
        {
            TestTypeFix<StructDeclarationSyntax>("struct", structName, expectedStructName);
        }

        private static void TestDelegateNameFix(string delegateName, string expectedDelegateName)
        {
            var delegateCode = "delegate void " + delegateName + "();";
            var expectedDelegateCode = "delegate void " + expectedDelegateName + "();";

            if (delegateName == expectedDelegateName)
                expectedDelegateCode = string.Empty;

            TestHelper.TestCodeFix<DelegateDeclarationSyntax>(new TypeIdentifierConventionRefactoring(), delegateCode, expectedDelegateCode);
        }

        private static void TestTypeFix<T>(string keyword, string inputName, string outputName)
        {
            var typeCode = keyword + " " + inputName + "{}";
            var expectedOutputCode = keyword + " " + outputName + "{}";

            if (inputName == outputName)
            {
                expectedOutputCode = string.Empty;
            }

            TestHelper.TestCodeFix<T>(new TypeIdentifierConventionRefactoring(), typeCode, expectedOutputCode);
        }
    }
}