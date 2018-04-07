using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Refactoring.DictionaryRefactorings
{
    public sealed class ClassNameNounRefactoring : Dictionary, IRefactoring
	{
		public string DiagnosticId => "SASKIA220";
		public string Title => DiagnosticId;
		public string Description => "Class name must be a noun";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration };

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
			var classNode = (ClassDeclarationSyntax)node;
			var identifierText = classNode.Identifier.Text;
			var lastWord = WordSplitter.GetLastWord(identifierText);

            if (!IsNoun(Database, lastWord))
            {
				return DiagnosticInfo.CreateFailedResult($"{Description}: {lastWord}.", markableLocation: classNode.Identifier.GetLocation());
			}
			return DiagnosticInfo.CreateSuccessfulResult();
        }

        private bool IsNoun(SQLiteConnection dbConnection, string word)
        {
            using (SQLiteCommand getword = new SQLiteCommand(dbConnection))
            {
                getword.CommandText = $"select * from entries where word = '{word}'";
                var reader = getword.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(1).StartsWith("n")) return true;
                }
                return false;
            }
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
		{
			var classNode = (ClassDeclarationSyntax)node;
			var identifierText = classNode.Identifier.Text;
			var hunspell = new HunspellEngine();
			var suggestion = GetSuggestionList(identifierText, hunspell).First();

			if (suggestion == null)
				return null;

			return new[] { classNode.ReplaceToken(classNode.Identifier, SyntaxFactory.Identifier(suggestion)) };
		}

		private IEnumerable<string> GetSuggestionList(string word, HunspellEngine hunspell)
		{
			const int displayWordLimit = 5;
			return hunspell.GetSuggestions(word).Take(displayWordLimit);
		}

		private string GetSuggestions(string word, HunspellEngine hunspell)
		{
			var suggestionsRefactored = GetSuggestionList(word, hunspell).Aggregate((x, y) => $"{x}\r\n{y}");
			if (!string.IsNullOrEmpty(suggestionsRefactored)) suggestionsRefactored = "Suggestions:\n" + suggestionsRefactored;
			return suggestionsRefactored;
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}
	}
}
