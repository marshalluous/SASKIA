using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class EnumDeclarationSyntaxStrategy : ClassTypeDeclarationSyntaxStrategy
    {
		internal override IEnumerable<string> IgnorableWords => new List<string>();
		internal override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> { { "_", new List<string> { "" } } };

		internal override string NamePrefix => string.Empty;

		internal override Type BaseType { get; }

		public EnumDeclarationSyntaxStrategy(Type baseType)
		{
			BaseType = baseType;
		}

		internal override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((EnumDeclarationSyntax)syntaxNode).Identifier;
		}
	}
}
