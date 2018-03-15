using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NHunspell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Refactoring.DictionaryRefactorings
{
	public sealed class ClassNameRefactoring : IRefactoring
	{
		public string DiagnosticId => "SASKIA200";
		public string Title => DiagnosticId;
		public string Description => Title;

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration };

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
			var classNode = (ClassDeclarationSyntax)node;
			var className = classNode.Identifier.ValueText;

			bool error = false;

			using (var hunspell = new Hunspell(GetFileInProjectFolder("en_us.aff"), GetFileInProjectFolder("en_us.dic")))
			{
				bool allWordsSpelledCorrect = true;
				string lastWord = GetLastWord(className);
				var wordList = SplitCamelCase(className);
				SpellCheckAllWords(hunspell, wordList, ref allWordsSpelledCorrect);
				error = !allWordsSpelledCorrect && GetWordType(lastWord) != "Noun";
			}

			return error 
				? DiagnosticInfo.CreateFailedResult("Class name is no noun or has a typo")
				: DiagnosticInfo.CreateSuccessfulResult();
		}

		public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
		{
			yield return node;
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}

		private string GetFileInProjectFolder(string fileName)
		{
			var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			return $"{assemblyPath}\\..\\..\\{fileName}";
		}

		private string GetLastWord(string input)
		{
			var wordList = SplitCamelCase(input);
			return wordList[wordList.Count - 1];
		}

		private List<string> SplitCamelCase(string input)
		{
			string words = Regex.Replace(input, @"(\p{Ll})(\P{Ll})", "$1 $2");
			return words.Split(' ').ToList();
		}

		private void SpellCheckAllWords(Hunspell hunspell, List<string> wordList, ref bool spelledCorrect)
		{
			spelledCorrect = true;
			foreach (string word in wordList)
			{
				if (!spelledCorrect)
					return;
				else
					spelledCorrect = CheckSpell(hunspell, word);
			}
		}

		private bool IsSpelledCorrect(Hunspell hunspell, string input)
		{
			return hunspell.Spell(input);
		}

		private bool CheckSpell(Hunspell hunspell, string word)
		{
			if (!IsSpelledCorrect(hunspell, word))
			{
				Console.WriteLine("Warning: '" + word + "' spelled incorrectly");
				return false;
			}
			return true;
		}

		private string GetWordType(string word)
		{
			return "";
		}
	}
}
