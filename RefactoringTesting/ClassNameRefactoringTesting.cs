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
	public sealed class ClassNameRefactoringTesting
	{
		[TestMethod]
		public void SimpleClassNameTypoTest()
		{
			var source = "class Appartment {}";
			TypoTest(source, new[] { "class Apartment{}" });
		}
		
		private static void TypoTest(string source, IEnumerable<string> expectedWords)
		{
			var node = TestHelper.Compile(source);
            node = TestHelper.FindNodeOfType<ClassDeclarationSyntax>(node);
			var refactoring = new TypoRefactoring();
			var resultNodes = refactoring.GetFixableNodes(node);
            ListCompare(expectedWords.ToList(), resultNodes.Select(resultNode => resultNode.ToString()).ToList());
		}

		private static void ListCompare(IList<string> first, IList<string> second)
		{
			Assert.AreEqual(first.Count(), second.Count());

			for (var index = 0; index < first.Count(); ++index)
			{
				Assert.AreEqual(first[index], second[index]);
			}
		}
	}
}
