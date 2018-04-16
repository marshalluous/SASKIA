using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class EnumDeclarationSyntaxStrategy : TypoRefactoringStrategy
	{
		protected override IEnumerable<string> IgnorableWords => new List<string>();
		protected override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> { { "_", new List<string> { "" } } };

		protected override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((EnumDeclarationSyntax)syntaxNode).Identifier;
		}
	}
}
