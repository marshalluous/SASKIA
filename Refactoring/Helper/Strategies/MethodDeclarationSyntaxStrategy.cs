using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
    class MethodDeclarationSyntaxStrategy : MethodTypeDeclarationSyntaxStrategy
    {
		internal override IEnumerable<string> IgnorableWords => new List<string>();
		internal override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> {
			{ "_", new List<string> { "" } }
		};

		internal override string NamePrefix => string.Empty;

		internal override Type BaseType { get; }

		public MethodDeclarationSyntaxStrategy(Type baseType)
		{
			BaseType = baseType;
		}

		internal override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((MethodDeclarationSyntax)syntaxNode).Identifier;
		}
    }
}
