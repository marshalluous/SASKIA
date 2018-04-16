using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.DictionaryRefactorings;
using RefactoringTesting.Helper;

namespace RefactoringTesting
{
	[TestClass]
	public sealed class TypoRefactoringTesting
	{
		private static void  TypoTest<T>(string source, string expected)
		{
			TestHelper.TestCodeFix<T>(new TypoRefactoring(), source, expected);
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
			TypoTest<StructDeclarationSyntax>(source, "struct Apartment{}");
		}

		[TestMethod]
		public void SimpleEnumNameTypoTest()
		{
			var source = "enum Appartment {}";
			TypoTest<EnumDeclarationSyntax>(source,  "enum Apartment{}");
		}

		[TestMethod]
		public void SimpleClassNameTypoTest()
		{
			var source = "class Appartment {}";
			TypoTest<ClassDeclarationSyntax>(source, "class Apartment{}");
		}

		[TestMethod]
		public void SimplePropertyNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int RooomCount { get; set; }" +
				"}";
			TypoTest<PropertyDeclarationSyntax>(source, "private int RoomCount{ get; set; }");
		}

		[TestMethod]
		public void UnderlinedPropertyNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int _RoomCount { get; set; }" +
				"}";
			TypoTest<PropertyDeclarationSyntax>(source, "private int RoomCount{ get; set; }");
		}

		[TestMethod]
		public void SimpleLinkedPropertyFieldTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					"private int _rooomCount;" +
					"private int RoomCount { get { return _rooomCount; } set{ _rooomCount=value; } }" +
				"}";
			TypoTest<FieldDeclarationSyntax>(source, "private int _roomCount;");
		}

		[TestMethod]
		public void SimpleFieldNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int rooomCount;" +
				"}";
			TypoTest<VariableDeclaratorSyntax>(source, "roomCount");
		}

		[TestMethod]
		public void UnderlinedFieldNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int _rooomCount;" +
				"}";
			TypoTest<VariableDeclaratorSyntax>(source, "_roomCount");
		}

		[TestMethod]
		public void SimpleInterfaceNameTypoTest()
		{
			var source = "interface IAppartment {}";
			TypoTest<InterfaceDeclarationSyntax>(source, "interface IApartment{}");
		}

		[TestMethod]
		public void SimpleMethodNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private void RooomCount(){}" +
				"}";
			TypoTest<MethodDeclarationSyntax>(source, "private void RoomCount(){}");
		}
	}
}
