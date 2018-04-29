using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Refactoring.Helper.Strategies
{
    abstract class MethodTypeDeclarationSyntaxStrategy : AbstractRefactoringStrategy
    {
        internal override IEnumerable<SyntaxNode> EvaluateWordType(SyntaxNode syntaxNode, SQLiteConnection database)
        {
            return new[] { syntaxNode };
        }

        internal override DiagnosticInfo DiagnoseWordType(SQLiteConnection database, string identifierText, SyntaxToken syntaxToken, string description)
        {
            return DiagnosticInfo.CreateSuccessfulResult();
        }
    }
}
