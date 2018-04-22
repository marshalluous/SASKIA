using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Refactoring.Helper.Strategies
{
	abstract class DictionaryRefactoringStrategy : Dictionary
	{
		protected abstract IEnumerable<string> IgnorableWords { get; }
		protected abstract IDictionary<string, List<string>> DefaultSuggestions { get; }
		protected abstract SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode);
		protected virtual string NamePrefix => string.Empty;

		private TypoCheckResult CheckTypo(SyntaxNode syntaxNode, out SyntaxToken syntaxToken,
			out bool namePrefixPresent, out List<string> allWords)
		{
			syntaxToken = GetSyntaxToken(syntaxNode);
			var identifier = syntaxToken.Text;
			namePrefixPresent = false;

			if (NamePrefix != "" && identifier.StartsWith(NamePrefix))
			{
				identifier = identifier.Substring(NamePrefix.Length);
				namePrefixPresent = true;
			}

			allWords = WordSplitter.GetSplittedWordList(identifier);
			return EvaluateTypo(allWords);
		}

		public DiagnosticInfo DiagnoseTypo(SyntaxNode syntaxNode, string description)
		{
			var typoCheckResult = CheckTypo(syntaxNode, out var syntaxToken, out _, out _);

			if (!typoCheckResult.IsIdentifierCorrectable)
				return DiagnosticInfo.CreateSuccessfulResult();

			var suggestionsAsString = "Suggestions:\n" + typoCheckResult.Suggestions.Aggregate((x, y) => $"{x}\r\n{y}");
			return DiagnosticInfo.CreateFailedResult($"{description}: {typoCheckResult.AffectedWord}.\n{suggestionsAsString}", markableLocation: syntaxToken.GetLocation());
		}

		public DiagnosticInfo DiagnoseNoun(SyntaxNode syntaxNode, string description)
		{
			var classNode = GetSyntaxToken(syntaxNode);
			var identifierText = classNode.Text;
			var lastWord = WordSplitter.GetLastWord(identifierText);
			if (!new WordTypeChecker(Database).IsNoun(lastWord))
				return DiagnosticInfo.CreateFailedResult($"{description}: {lastWord}.", markableLocation: classNode.GetLocation());
			return DiagnosticInfo.CreateSuccessfulResult();
		}

		public IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode syntaxNode)
		{
			var typoCheckResult = CheckTypo(syntaxNode, out var syntaxToken, out var namePrefixPresent, out var allWords);
			
			if (!typoCheckResult.IsIdentifierCorrectable)
				return null;

			var suggestion = typoCheckResult.Suggestions.First();
			string newIdentifier = ConcatNewIdentifier(allWords, suggestion, typoCheckResult.AffectedWord);

			if (namePrefixPresent)
			{
				newIdentifier = NamePrefix + newIdentifier;
			}

			return new[] { syntaxNode.ReplaceToken(syntaxToken, SyntaxFactory.Identifier(newIdentifier)) };
		}

		public IEnumerable<SyntaxNode> EvaluateNounNodes(SyntaxNode syntaxNode)
		{
			return new[] { syntaxNode };
		}

		private static string ConcatNewIdentifier(List<string> allWords, string suggestion, string affectedWord)
		{
			int affectedIndex = allWords.FindIndex(word => word == affectedWord);
			return allWords
				.Select(word => (CompareIndex(allWords, word, affectedIndex) ? suggestion : word))
				.Aggregate((accumulated, word) => accumulated + word);
		}

		private static bool CompareIndex(List<string> allWords, string word, int affectedIndex)
		{
			return (allWords.FindIndex(w => w == word) == affectedIndex);
		}

		private TypoCheckResult EvaluateTypo(List<string> wordList)
		{
			var hunspell = new HunspellEngine();

			foreach (var word in wordList.Where(w => hunspell.HasTypo(w)))
			{
				if (IgnorableWords.Contains(word)) continue;
				return new TypoCheckResult(word, true, DefaultSuggestions.ContainsKey(word) ? DefaultSuggestions[word] : hunspell.GetSuggestions(word));
			}
			
			return new TypoCheckResult(string.Empty, false, null);
		}

		private sealed class TypoCheckResult
		{
			internal TypoCheckResult(string affectedWord, bool typoFound, List<string> suggestions)
			{
				AffectedWord = affectedWord;
				TypoFound = typoFound;
				Suggestions = suggestions;
			}

			public string AffectedWord { get; }
			public bool TypoFound { get; }
			public List<string> Suggestions { get; }
			public bool IsIdentifierCorrectable => TypoFound && Suggestions != null;
		}
	}
}
