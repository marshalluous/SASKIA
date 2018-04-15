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
		private static void  TypoTest<T>(string source, IEnumerable<string> expectedWords)
		{
			var node = TestHelper.Compile(source);
			node = TestHelper.FindNodeOfType<T>(node);
			var refactoring = new TypoRefactoring();
			var resultNodes = refactoring.GetFixableNodes(node);
			ListCompare(expectedWords.ToList(), resultNodes.Select(resultNode => resultNode.ToString()).ToList());
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
		public void SimpleClassNameTypoTest()
		{
			var source = "class Appartment {}";
			TypoTest<ClassDeclarationSyntax>(source, new[] { "class Apartment{}" });
		}

		[TestMethod]
		public void SimplePropertyNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int RooomCount { get; set; }" +
				"}";
			TypoTest<PropertyDeclarationSyntax>(source, new[] { "" +
				"private int RoomCount{ get; set; }"});
		}

		[TestMethod]
		public void UnderlinedPropertyNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int _RoomCount { get; set; }" +
				"}";
			TypoTest<PropertyDeclarationSyntax>(source, new[] { "" +
				"private int RoomCount{ get; set; }"});
		}

		[TestMethod]
		public void SimpleFieldNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int rooomCount;" +
				"}";
			TypoTest<VariableDeclaratorSyntax>(source, new[] { "" +
				"roomCount;"});
		}

		[TestMethod]
		public void UnderlinedFieldNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private int _rooomCount;" +
				"}";
			TypoTest<VariableDeclaratorSyntax>(source, new[] { "" +
				"_roomCount;"});
		}

		[TestMethod]
		public void SimpleInterfaceNameTypoTest()
		{
			var source = "interface IAppartment {}";
			TypoTest<InterfaceDeclarationSyntax>(source, new[] { "interface IApartment{}" });
		}

		[TestMethod]
		public void SimpleMethodNameTypoTest()
		{
			var source = "" +
				"class Apartment {" +
					" private void RooomCount(){}" +
				"}";
			TypoTest<VariableDeclaratorSyntax>(source, new[] { "private void RoomCount(){}" });
		}
	}
}
