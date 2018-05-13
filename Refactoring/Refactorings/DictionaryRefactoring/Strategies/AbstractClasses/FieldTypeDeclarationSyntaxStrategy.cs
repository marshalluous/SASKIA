﻿using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.CodeAnalysis;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses
{
    internal abstract class FieldTypeDeclarationSyntaxStrategy : AbstractRefactoringStrategy
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