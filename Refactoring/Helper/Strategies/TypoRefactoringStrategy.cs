using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Refactoring.Helper.Strategies
{
	abstract class TypoRefactoringStrategy
	{
		public DiagnosticInfo Diagnose(SyntaxNode syntaxNode, string description)
		{
			var syntaxToken = GetSyntaxToken(syntaxNode);
			bool hasTypo;
			string affectedWord;
			List<string> suggestions;
			EvaluateTypo(WordSplitter.GetSplittedWordList(syntaxToken.Text), out affectedWord, out hasTypo, out suggestions);
			if (hasTypo)
			{
				var suggestionsAsString = "";
				if (suggestions != null)
					suggestionsAsString = "Suggestions:\n" + suggestions.Aggregate((x, y) => $"{x}\r\n{y}");
				return DiagnosticInfo.CreateFailedResult($"{description}: {affectedWord}.\n{suggestionsAsString}", markableLocation: syntaxToken.GetLocation());
			}
			else
				return DiagnosticInfo.CreateSuccessfulResult();
		}

		protected virtual void EvaluateTypo(List<string> wordList, out string affectedWord, out bool hasTypo, out List<string> suggestions)
		{
			RemoveIgnorableWords(wordList);
			Evaluate(wordList, out affectedWord, out hasTypo, out suggestions);
		}

		private static void Evaluate(List<string> wordList, out string affectedWord, out bool hasTypo, out List<string> suggestions)
		{
			suggestions = null;
			hasTypo = false;
			affectedWord = "";
			var hunspell = new HunspellEngine();
			foreach (var word in wordList.Where(w => hunspell.HasTypo(w)))
			{
				hasTypo = true;
				affectedWord = word;
				suggestions = hunspell.GetSuggestions(word);
				return;
			}
		}

		protected void SafeEvaluateTypo(List<string> wordList, out string affectedWord, out bool hasTypo, out List<string> suggestions)
		{
			Evaluate(wordList, out affectedWord, out hasTypo, out suggestions);
		}

		public IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode node)
		{
			var identifier = GetSyntaxToken(node);
			bool hasTypo;
			string affectedWord = "";
			List<string> suggestions;
			var allWords = WordSplitter.GetSplittedWordList(identifier.Text);
			SafeEvaluateTypo(allWords, out affectedWord, out hasTypo, out suggestions);
			if (suggestions == null)
				return null;
			int affectedIndex = allWords.FindIndex(w => w == affectedWord);
			var suggestion = suggestions.First();
			var newIdentifier = "";
			foreach (var word in allWords)
			{
				var temp = word;
				if (allWords.FindIndex(w => w == temp) == affectedIndex)
				{
					temp = suggestion;
				}
				newIdentifier += temp;
			}
			return new[] { node.ReplaceToken(identifier, SyntaxFactory.Identifier(newIdentifier)) };
		}

		protected abstract SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode);

		protected abstract void RemoveIgnorableWords(List<string> wordList);
	}
}
