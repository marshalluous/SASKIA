using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Refactoring.Helper.Strategies
{
    abstract class ClassTypeDeclarationSyntaxStrategy : AbstractRefactoringStrategy
    {
        internal override IEnumerable<SyntaxNode> EvaluateWordType(SyntaxNode syntaxNode, SQLiteConnection database)
        {
            var hunspell = new HunspellEngine();
            var syntaxToken = GetSyntaxToken(syntaxNode);
            var identifier = syntaxToken.Text;

            if (NamePrefix != string.Empty)
            {
                identifier = identifier.Remove(0, NamePrefix.Length);
            }

            var lastWord = WordSplitter.GetLastWord(identifier);
            var suggestions = hunspell.GetSuggestions(lastWord);
            var wordTypechecker = new WordTypeChecker(database);
            var clearedSuggestions = FilterNouns(suggestions, wordTypechecker);

            if (clearedSuggestions.Count > 0)
            {
                var newIdentifier = syntaxToken.Text.Replace(lastWord, clearedSuggestions.Last());
                return new[] { syntaxNode.ReplaceToken(syntaxToken, SyntaxFactory.Identifier(newIdentifier)) };
            }
            else
            {
                return new[] { syntaxNode };
            }
        }

        private List<string> FilterNouns(List<string> suggestions, WordTypeChecker wordTypechecker)
        {
            var clearedSuggestions = new List<string>();
            var counter = 0;
            foreach (var word in suggestions)
            {
                if (wordTypechecker.IsNoun(word))
                {
                    clearedSuggestions.Add(word);
                    if (++counter >= 5) break;
                }
            }

            return clearedSuggestions;
        }

        internal override DiagnosticInfo DiagnoseWordType(SQLiteConnection database, string identifierText, SyntaxToken syntaxToken, string description)
        {
            var lastWord = WordSplitter.GetLastWord(identifierText);
            if (!new WordTypeChecker(database).IsNoun(lastWord))
                return DiagnosticInfo.CreateFailedResult($"{description}: Missing noun in identifier", markableLocation: syntaxToken.GetLocation());
            return DiagnosticInfo.CreateSuccessfulResult();
        }
    }
}
