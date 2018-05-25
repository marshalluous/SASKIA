﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Refactoring.Helper;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses;
using Refactoring.WordHelper;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies
{
	internal sealed class TypoRefactoringStrategy : DictionaryClass, IRefactoringBaseStrategy
	{
		private AbstractRefactoringStrategy strategy;

		private TypoCheckResult CheckTypo(SyntaxNode syntaxNode, out SyntaxToken syntaxToken,
			out bool namePrefixPresent, out List<string> allWords)
		{
			syntaxToken = strategy.GetSyntaxToken(syntaxNode);
			var identifier = syntaxToken.Text;
			namePrefixPresent = false;

			if (strategy.NamePrefix != "" && identifier.StartsWith(strategy.NamePrefix))
			{
				identifier = identifier.Substring(strategy.NamePrefix.Length);
				namePrefixPresent = true;
			}

			allWords = WordSplitter.GetSplittedWordList(identifier).ToList();
			return EvaluateTypo(allWords);
		}

		public DiagnosticInfo Diagnose(SyntaxNode syntaxNode, string description)
		{
			var typoCheckResult = CheckTypo(syntaxNode, out var syntaxToken, out _, out _);

			if (!typoCheckResult.IsIdentifierCorrectable)
				return DiagnosticInfo.CreateSuccessfulResult();

		    const string additionalInfo = nameof(TypoRefactoringStrategy) + "." + nameof(Diagnose);
            var suggestionsAsString = "Suggestions:\n" + typoCheckResult.Suggestions.Aggregate((x, y) => $"{x}\r\n{y}");
			return DiagnosticInfo.CreateFailedResult($"{description}: {typoCheckResult.AffectedWord}.\n{suggestionsAsString}",
			    additionalInfo, syntaxToken.GetLocation());
		}

		public IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode syntaxNode)
		{
			var typoCheckResult = CheckTypo(syntaxNode, out var syntaxToken, out var namePrefixPresent, out var allWords);
			
			if (!typoCheckResult.IsIdentifierCorrectable)
				return null;

			var suggestion = typoCheckResult.Suggestions.First();
			var newIdentifier = ConcatNewIdentifier(allWords, suggestion, typoCheckResult.AffectedWord);

			if (namePrefixPresent)
			{
				newIdentifier = strategy.NamePrefix + newIdentifier;
			}

			return new[] { syntaxNode.ReplaceToken(syntaxToken, SyntaxFactory.Identifier(newIdentifier)) };
		}

		private static string ConcatNewIdentifier(List<string> allWords, string suggestion, string affectedWord)
		{
			var affectedIndex = allWords.FindIndex(word => word == affectedWord);
			return allWords
				.Select(word => CompareIndex(allWords, word, affectedIndex) ? suggestion : word)
				.Aggregate((accumulated, word) => accumulated + word);
		}

		private static bool CompareIndex(List<string> allWords, string word, int affectedIndex)
		{
			return allWords.FindIndex(w => w == word) == affectedIndex;
		}

		private TypoCheckResult EvaluateTypo(IEnumerable<string> wordList)
		{
			var hunspell = new HunspellEngine();
		    var checkableWordList = wordList
		        .Where(word => word.Length > 1)
		        .Where(word => !strategy.IgnorableWords.Contains(word));

			foreach (var word in checkableWordList.Where(w => hunspell.HasTypo(w)))
			{
				return new TypoCheckResult(word, true, strategy.DefaultSuggestions.ContainsKey(word) ? strategy.DefaultSuggestions[word] : hunspell.GetSuggestions(word,5));
			}
			
			return new TypoCheckResult(string.Empty, false, null);
		}


		public void RegisterStrategy(AbstractRefactoringStrategy newStrategy)
		{
		    strategy = newStrategy;
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
		    private bool TypoFound { get; }
			public List<string> Suggestions { get; }
			public bool IsIdentifierCorrectable => TypoFound && Suggestions != null;
		}
	}
}
