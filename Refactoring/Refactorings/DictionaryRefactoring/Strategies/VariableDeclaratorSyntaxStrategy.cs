using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies
{
    internal sealed class VariableDeclaratorSyntaxStrategy : FieldTypeDeclarationSyntaxStrategy
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
	}
}
