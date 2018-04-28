using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.CodeAnalysis;

namespace Refactoring.Helper.Strategies
{
    public abstract class AbstractRefactoringStrategy
	{
		internal abstract IEnumerable<string> IgnorableWords { get; }
		internal abstract IDictionary<string, List<string>> DefaultSuggestions { get; }
		internal abstract string NamePrefix { get; }
		internal abstract SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode);
		internal abstract Type BaseType { get; }

        internal abstract IEnumerable<SyntaxNode> EvaluateWordType(SyntaxNode syntaxNode, SQLiteConnection database);

        internal abstract DiagnosticInfo DiagnoseWordType(SQLiteConnection database, string identifierText, SyntaxToken syntaxToken, string description);
	}
}
