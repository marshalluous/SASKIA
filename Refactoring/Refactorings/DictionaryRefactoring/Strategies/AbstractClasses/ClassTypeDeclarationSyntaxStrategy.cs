using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;
using Refactoring.WordHelper;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses
{
    internal abstract class ClassTypeDeclarationSyntaxStrategy : AbstractRefactoringStrategy
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

            return new[] { syntaxNode };
        }

        private static List<string> FilterNouns(List<string> suggestions, WordTypeChecker wordTypechecker)
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
            return !new WordTypeChecker(database).IsNoun(lastWord) ?
                CreateFailedDiagnosticResult(syntaxToken, description) :
                DiagnosticInfo.CreateSuccessfulResult();
        }

        private DiagnosticInfo CreateFailedDiagnosticResult(SyntaxToken syntaxToken, string description)
        {
            const string additionalInfo = nameof(ClassTypeDeclarationSyntaxStrategy) + "." + nameof(DiagnoseWordType);
            return DiagnosticInfo.CreateFailedResult($"{description}: Missing noun in identifier",
                additionalInfo, syntaxToken.GetLocation());
        }
    }
}