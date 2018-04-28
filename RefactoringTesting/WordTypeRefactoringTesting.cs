using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring;
using Refactoring.DictionaryRefactorings;
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
            var source = "class Downey {}";
            WordTypeTest<ClassDeclarationSyntax>(new WordTypeRefactoring(), source, "class Downed{}");
        }

        [TestMethod]
        public void ClassNounTest()
        {
            var source = "class ApartmentDowney {}";
            WordTypeTest<ClassDeclarationSyntax>(new WordTypeRefactoring(), source, "class ApartmentDowned{}");
        }

        [TestMethod]
        public void SimpleInterfaceNounTest()
        {
            var source = "interface IDowney {}";
            WordTypeTest<InterfaceDeclarationSyntax>(new WordTypeRefactoring(), source, "interface IDowned{}"); 
        }

        [TestMethod]
        public void InterfaceNounTest()
        {
            var source = "interface IApartmentDowney {}";
            WordTypeTest<InterfaceDeclarationSyntax>(new WordTypeRefactoring(), source, "interface IApartmentDowned{}");
        }

        [TestMethod]
        public void SimpleStructNounTest()
        {
            var source = "struct Downey {}";
            WordTypeTest<StructDeclarationSyntax>(new WordTypeRefactoring(), source, "struct Downed{}");
        }

        [TestMethod]
        public void StructNounTest()
        {
            var source = "struct ApartmentDowney {}";
            WordTypeTest<StructDeclarationSyntax>(new WordTypeRefactoring(), source, "struct ApartmentDowned {}");
        }

        [TestMethod]
        public void SimpleEnumNounTest()
        {
            var source = "enum Downey {}";
            WordTypeTest<EnumDeclarationSyntax>(new WordTypeRefactoring(), source, "enum Downed {}");
        }

        [TestMethod]
        public void EnumNounTest()
        {
            var source = "enum ApartmentDowney {}";
            WordTypeTest<EnumDeclarationSyntax>(new WordTypeRefactoring(), source, "enum ApartmentDowned {}");
        }
    }
}
