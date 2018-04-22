using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class FieldDeclarationSyntaxStrategy : AbstractRefactoringStrategy
	{
		internal override IEnumerable<string> IgnorableWords => new List<string>();
		internal override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> { };
		internal override string NamePrefix => "_";

		internal override Type BaseType { get; }

		public FieldDeclarationSyntaxStrategy(Type baseType)
		{
			BaseType = baseType;
		}

		internal override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((FieldDeclarationSyntax)syntaxNode).Declaration.Variables.First().Identifier;
		}
	}
}
