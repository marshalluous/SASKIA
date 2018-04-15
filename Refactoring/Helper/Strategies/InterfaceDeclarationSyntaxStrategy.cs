using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class InterfaceDeclarationSyntaxStrategy : TypoRefactoringStrategy
	{
		protected override List<string> IgnorableWords => new List<string> { "I" };
		protected override Dictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> {
			{ "_", new List<string> { "" } }
		};

		protected override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((InterfaceDeclarationSyntax)syntaxNode).Identifier;
		}
	}
}
