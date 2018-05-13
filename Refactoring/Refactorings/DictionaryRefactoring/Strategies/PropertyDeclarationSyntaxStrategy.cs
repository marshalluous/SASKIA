﻿using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies
{
    internal sealed class PropertyDeclarationSyntaxStrategy : FieldTypeDeclarationSyntaxStrategy
    {
		internal override IEnumerable<string> IgnorableWords => new List<string>();
		internal override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> {
			{ "_", new List<string> { "" } }
		};

		internal override string NamePrefix => string.Empty;

		internal override Type BaseType { get; }

		public PropertyDeclarationSyntaxStrategy(Type baseType)
		{
			BaseType = baseType;
		}

		internal override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((PropertyDeclarationSyntax)syntaxNode).Identifier;
		}
	}
}