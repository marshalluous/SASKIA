using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies
{
    internal sealed class InterfaceDeclarationSyntaxStrategy : ClassTypeDeclarationSyntaxStrategy
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
