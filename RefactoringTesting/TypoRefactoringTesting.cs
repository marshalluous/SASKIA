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
	public sealed class TypoRefactoringTesting
	{
		private static void  TypoTest<T>(IRefactoring refactoring, string source, string expected)
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
		public void SimpleStructNameTypoTest()
		{
			var source = "struct Appartment {}";
			TypoTest<StructDeclarationSyntax>(new TypoRefactoring(), source, "struct Apartment{}");
		}

		[TestMethod]
		public void SimpleEnumNameTypoTest()
		{
			var source = "enum Appartment {}";
			TypoTest<EnumDeclarationSyntax>(new TypoRefactoring(), source,  "enum Apartment{}");
		}

		[TestMethod]
		public void SimpleClassNameTypoTest()
		{
			var source = "class Appartment {}";
			TypoTest<ClassDeclarationSyntax>(new TypoRefactoring(), source, "class Apartment{}");
		}

		[TestMethod]
		public void SimplePropertyNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int RooomCount { get; set; }" +
				"}";
			TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, "private int RoomCount{ get; set; }");
		}

		[TestMethod]
		public void UnderlinedPropertyNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int _RoomCount { get; set; }" +
				"}";
			TypoTest<PropertyDeclarationSyntax>(new TypoRefactoring(), source, "private int RoomCount{ get; set; }");
		}

		[TestMethod]
		public void SimpleLinkedPropertyFieldTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					"private int _rooomCount;" +
					"private int RoomCount { get { return _rooomCount; } set{ _rooomCount=value; } }" +
				"}";
			TypoTest<FieldDeclarationSyntax>(new TypoRefactoring(), source, "private int _roomCount;");
		}

		[TestMethod]
		public void SimpleFieldNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int rooomCount;" +
				"}";
			TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "roomCount");
		}

		[TestMethod]
		public void UnderlinedFieldNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int _rooomCount;" +
				"}";
			TypoTest<VariableDeclaratorSyntax>(new TypoRefactoring(), source, "_roomCount");
		}

		[TestMethod]
		public void SimpleInterfaceNameTypoTest()
		{
			var source = "interface IAppartment {}";
			TypoTest<InterfaceDeclarationSyntax>(new TypoRefactoring(), source, "interface IApartment{}");
		}

		[TestMethod]
		public void SimpleMethodNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private void RooomCount(){}" +
				"}";
			TypoTest<MethodDeclarationSyntax>(new TypoRefactoring(), source, "private void RoomCount(){}");
		}

		[TestMethod]
		public void SimpleClassNounTest()
		{
			var source = "class Downey {}";
			TypoTest<ClassDeclarationSyntax>(new WordTypeRefactoring(), source, "class Down{}"); // no suggestion is being provided
		}

		[TestMethod]
		public void SimpleInterfaceNounTest()
		{
			var source = "interface IApartmentDo {}";
			TypoTest<InterfaceDeclarationSyntax>(new WordTypeRefactoring(), source, "interface IApartmentDo {}"); // no suggestion is being provided
		}

		[TestMethod]
		public void SimpleStructNounTest()
		{
			var source = "struct ApartmentDo {}";
			TypoTest<StructDeclarationSyntax>(new WordTypeRefactoring(), source, "struct ApartmentDo {}"); // no suggestion is being provided
		}

		[TestMethod]
		public void SimpleEnumNounTest()
		{
			var source = "enum ApartmentDo {}";
			TypoTest<EnumDeclarationSyntax>(new WordTypeRefactoring(), source, "enum ApartmentDo {}"); // no suggestion is being provided
		}
	}
}
