using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class InterfaceDeclarationSyntaxStrategy : AbstractRefactoringStrategy
	{
		internal override IEnumerable<string> IgnorableWords => new List<string> { "I" };
		internal override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> {
			{ "_", new List<string> { "" } }
		};
		internal override string NamePrefix => "I";
		internal override Type BaseType { get; }

		public InterfaceDeclarationSyntaxStrategy(Type baseType)
		{
			BaseType = baseType;
		}

		internal override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((InterfaceDeclarationSyntax)syntaxNode).Identifier;
		}
	}
}
