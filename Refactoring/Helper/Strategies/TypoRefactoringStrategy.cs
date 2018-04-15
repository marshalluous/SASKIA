using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Refactoring.Helper.Strategies
{
	abstract class TypoRefactoringStrategy
	{
		protected abstract List<string> IgnorableWords { get; }
		protected abstract Dictionary<string, List<string>> DefaultSuggestions { get; }

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
			suggestions = null;
			hasTypo = false;
			affectedWord = "";
			var hunspell = new HunspellEngine();
			foreach (var word in wordList.Where(w => hunspell.HasTypo(w)))
			{
				if (IgnorableWords.Contains(word)) continue;
				hasTypo = true;
				affectedWord = word;
				if (DefaultSuggestions.ContainsKey(word))
				{
					suggestions = DefaultSuggestions[word];
					return;
				}
				suggestions = hunspell.GetSuggestions(word);
				return;
			}
		}

		public IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode node)
		{
			var identifier = GetSyntaxToken(node);
			bool hasTypo;
			string affectedWord = "";
			List<string> suggestions;
			var allWords = WordSplitter.GetSplittedWordList(identifier.Text);
			EvaluateTypo(allWords, out affectedWord, out hasTypo, out suggestions);
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
	}
}
