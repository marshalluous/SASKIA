using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Refactoring.Helper.Strategies
{
	public abstract class AbstractRefactoringStrategy
	{
		internal abstract IEnumerable<string> IgnorableWords { get; }
		internal abstract IDictionary<string, List<string>> DefaultSuggestions { get; }
		internal abstract string NamePrefix { get; }
		internal abstract SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode);
		internal abstract Type BaseType { get; }
		internal IEnumerable<SyntaxNode> EvaluateWordType(SyntaxNode syntaxNode, SQLiteConnection database)
		{
			var hunspell = new HunspellEngine();
			var syntaxToken = GetSyntaxToken(syntaxNode);
			var lastWord = WordSplitter.GetLastWord(syntaxToken.Text);
			var suggestions = hunspell.GetSuggestions(lastWord);
			var clearedSuggestions = new List<string>();
			var wordTypechecker = new WordTypeChecker(database);
			foreach (var word in suggestions)
			{
				if (!wordTypechecker.IsVerb(word) &&
					!wordTypechecker.IsAdverb(word)
					// && is no Adjective etc
					)
					clearedSuggestions.Add(word);
			}

			return new[] { syntaxNode.ReplaceToken(syntaxToken, SyntaxFactory.Identifier(suggestions.Last())) }; 
			
			// TypoCheckResult(word, true, strategy.DefaultSuggestions.ContainsKey(word) ? strategy.DefaultSuggestions[word] : hunspell.GetSuggestions(word));

			//return new TypoCheckResult(string.Empty, false, null);

			//return new[] { syntaxNode };
		}

		internal virtual DiagnosticInfo DiagnoseWordType(SQLiteConnection database, string identifierText, SyntaxToken syntaxToken, string description)
		{
			var lastWord = WordSplitter.GetLastWord(identifierText);
			if (!new WordTypeChecker(database).IsNoun(lastWord))
				return DiagnosticInfo.CreateFailedResult($"{description}: Class name must be a noun.", markableLocation: syntaxToken.GetLocation());
			return DiagnosticInfo.CreateSuccessfulResult();
		}
	}
}
