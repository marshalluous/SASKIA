using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class VariableDeclaratorSyntaxStrategy : AbstractRefactoringStrategy
	{
		internal override IEnumerable<string> IgnorableWords => new List<string> { "_" };
		internal override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>>();

		internal override string NamePrefix => string.Empty;

		internal override Type BaseType { get; }

		public VariableDeclaratorSyntaxStrategy(Type baseType)
		{
			BaseType = baseType;
		}

		internal override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((VariableDeclaratorSyntax)syntaxNode).Identifier;
		}

		internal override DiagnosticInfo DiagnoseWordType(SQLiteConnection database, string identifierText, SyntaxToken syntaxToken, string description)
		{
			return DiagnosticInfo.CreateSuccessfulResult();
		}
	}
}
