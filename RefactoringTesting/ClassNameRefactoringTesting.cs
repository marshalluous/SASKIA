using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refactoring.DictionaryRefactorings;

namespace RefactoringTesting
{
	[TestClass]
	public sealed class ClassNameRefactoringTesting
	{
		[TestMethod]
		public void SimpleClassNameTypoTest()
		{
			var source = "class Fisch {}";
			TypoTest(source, new[] { "Fish" });
		}
		
		private static void TypoTest(string source, IEnumerable<string> expectedWords)
		{
			var node = Compile(source);
			var refactoring = new ClassNameRefactoring();
			var resultNodes = refactoring.GetFixableNodes(node);
			Assert.AreEqual(expectedWords, resultNodes.Select(resultNode => resultNode.ToString()));
		}

		private static void ListCompare(IList<string> first, IList<string> second)
		{
			Assert.AreEqual(first.Count(), second.Count());

			for (var index = 0; index < first.Count(); ++index)
			{
				Assert.AreEqual(first[index], second[index]);
			}
		}

		private static SyntaxNode Compile(string source)
		{
			return CSharpSyntaxTree.ParseText(source)
				.GetRoot();
		}
	}
}
