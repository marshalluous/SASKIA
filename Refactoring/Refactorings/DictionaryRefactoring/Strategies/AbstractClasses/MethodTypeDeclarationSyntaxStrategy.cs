using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Microsoft.CodeAnalysis;
using Refactoring.Helper;
using Refactoring.WordHelper;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses
{
    internal abstract class MethodTypeDeclarationSyntaxStrategy : AbstractRefactoringStrategy
    {
        internal override DiagnosticInfo DiagnoseWordType(SQLiteConnection database, string identifierText, SyntaxToken syntaxToken, string description)
        {
            var wordTypeChecker = new WordTypeChecker(database);
            var words = WordSplitter.GetSplittedWordList(identifierText);
            var hasVerb = words.Any(wordTypeChecker.IsVerb);
            var additionalInfo = nameof(MethodTypeDeclarationSyntaxStrategy) + "." + nameof(DiagnoseWordType);
            return hasVerb ?
                DiagnosticInfo.CreateSuccessfulResult() : 
                DiagnosticInfo.CreateFailedResult($"Missing verb in identifier", additionalInfo, syntaxToken.GetLocation());
        }

        internal override IEnumerable<SyntaxNode> EvaluateWordType(SyntaxNode syntaxNode, SQLiteConnection database)
        {
            return new[] { syntaxNode };
        }
    }
}
