using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring;
using Refactoring.Refactorings.DictionaryRefactoring;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
    [TestClass]
	public sealed class WordTypeRefactoringTesting
    {
		private static void  WordTypeTest<T>(IRefactoring refactoring, string source, string expected)
		{
			TestHelper.TestCodeFix<T>(refactoring, source, expected);
		}

		private static void ListCompare(IList<string> first, IList<string> second)
		{
			Assert.IsTrue(first != null && second != null && first.Count() == second.Count());

			for (var index = 0; index < first.Count(); ++index)
			{
				Assert.AreEqual(first[index], second[index]);
			}
        }

        [TestMethod]
        public void SimpleClassNounTest()
        {
            var source = "class Mustering {}";
            WordTypeTest<ClassDeclarationSyntax>(new WordTypeRefactoring(), source, "class Muttering{}");
        }

        [TestMethod]
        public void ClassNounTest()
        {
            var source = "class ApartmentMustering{}";
            WordTypeTest<ClassDeclarationSyntax>(new WordTypeRefactoring(), source, "class ApartmentMuttering{}");
        }

        [TestMethod]
        public void ClassUnderlineNounTest()
        {
            var source = "class Apartment_Mustering{}";
            WordTypeTest<ClassDeclarationSyntax>(new WordTypeRefactoring(), source, "class Apartment_Muttering{}");
        }

        [TestMethod]
        public void SimpleInterfaceNounTest()
        {
            var source = "interface IMustering {}";
            WordTypeTest<InterfaceDeclarationSyntax>(new WordTypeRefactoring(), source, "interface IMuttering{}");
        }

        [TestMethod]
        public void SimpleInterfaceUnderlineNounTest()
        {
            var source = "interface I_Mustering {}";
            WordTypeTest<InterfaceDeclarationSyntax>(new WordTypeRefactoring(), source, "interface I_Muttering{}");
        }

        [TestMethod]
        public void InterfaceNounTest()
        {
            var source = "interface IApartmentMustering {}";
            WordTypeTest<InterfaceDeclarationSyntax>(new WordTypeRefactoring(), source, "interface IApartmentMuttering{}");
        }

        [TestMethod]
        public void InterfaceUnderlineNounTest()
        {
            var source = "interface IApartment_Mustering {}";
            WordTypeTest<InterfaceDeclarationSyntax>(new WordTypeRefactoring(), source, "interface IApartment_Muttering{}");
        }

        [TestMethod]
        public void InterfaceDoubleUnderlineNounTest()
        {
            var source = "interface I_Apartment_Mustering {}";
            WordTypeTest<InterfaceDeclarationSyntax>(new WordTypeRefactoring(), source, "interface I_Apartment_Muttering{}");
        }

        [TestMethod]
        public void SimpleStructNounTest()
        {
            var source = "struct Mustering {}";
            WordTypeTest<StructDeclarationSyntax>(new WordTypeRefactoring(), source, "struct Muttering{}");
        }

        [TestMethod]
        public void StructNounTest()
        {
            var source = "struct ApartmentMustering {}";
            WordTypeTest<StructDeclarationSyntax>(new WordTypeRefactoring(), source, "struct ApartmentMuttering{}");
        }

        [TestMethod]
        public void StructUnderlineNounTest()
        {
            var source = "struct Apartment_Mustering {}";
            WordTypeTest<StructDeclarationSyntax>(new WordTypeRefactoring(), source, "struct Apartment_Muttering{}");
        }

        [TestMethod]
        public void SimpleEnumNounTest()
        {
            var source = "enum Mustering {}";
            WordTypeTest<EnumDeclarationSyntax>(new WordTypeRefactoring(), source, "enum Muttering{}");
        }

        [TestMethod]
        public void EnumNounTest()
        {
            var source = "enum ApartmentMustering {}";
            WordTypeTest<EnumDeclarationSyntax>(new WordTypeRefactoring(), source, "enum ApartmentMuttering{}");
        }

        [TestMethod]
        public void EnumUnderlineNounTest()
        {
            var source = "enum Apartment_Mustering {}";
            WordTypeTest<EnumDeclarationSyntax>(new WordTypeRefactoring(), source, "enum Apartment_Muttering{}");
        }

        [TestMethod]
        public void FieldNounTest()
        {
            var source = "enum ApartmentMustering {}";
            WordTypeTest<EnumDeclarationSyntax>(new WordTypeRefactoring(), source, "enum ApartmentMuttering{}");
        }

        [TestMethod]
        public void FieldUnderlineNounTest()
        {
            var source = "enum Apartment_Mustering {}";
            WordTypeTest<EnumDeclarationSyntax>(new WordTypeRefactoring(), source, "enum Apartment_Muttering{}");
        }
    }
}
