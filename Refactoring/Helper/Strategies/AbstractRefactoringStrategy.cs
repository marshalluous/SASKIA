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

        //public IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode syntaxNode)
        //{
        //    var typoCheckResult = CheckTypo(syntaxNode, out var syntaxToken, out var namePrefixPresent, out var allWords);

        //    if (!typoCheckResult.IsIdentifierCorrectable)
        //        return null;

        //    var suggestion = typoCheckResult.Suggestions.First();
        //    string newIdentifier = ConcatNewIdentifier(allWords, suggestion, typoCheckResult.AffectedWord);

        //    if (namePrefixPresent)
        //    {
        //        newIdentifier = strategy.NamePrefix + newIdentifier;
        //    }

        //    return new[] { syntaxNode.ReplaceToken(syntaxToken, SyntaxFactory.Identifier(newIdentifier)) };
        //}

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
                if (IsANoun(word, wordTypechecker))
                {
                    clearedSuggestions.Add(word);
                }
            }

            return new[] { syntaxNode.ReplaceToken(syntaxToken, SyntaxFactory.Identifier(suggestions.Last())) };
		}

        private bool IsANoun(string word, WordTypeChecker wordTypechecker)
        {
            return !wordTypechecker.IsVerb(word) &&
                   !wordTypechecker.IsAdverb(word) &&
                   !wordTypechecker.IsAdjective(word) &&
                   wordTypechecker.IsNoun(word);
        }

        internal virtual DiagnosticInfo DiagnoseWordType(SQLiteConnection database, string identifierText, SyntaxToken syntaxToken, string description)
		{
			var lastWord = WordSplitter.GetLastWord(identifierText);
			if (!new WordTypeChecker(database).IsNoun(lastWord))
				return DiagnosticInfo.CreateFailedResult($"{description}: Missing noun in identifier", markableLocation: syntaxToken.GetLocation());
			return DiagnosticInfo.CreateSuccessfulResult();
		}
	}
}
