using System;
using System.Collections.Generic;
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
	}
}
