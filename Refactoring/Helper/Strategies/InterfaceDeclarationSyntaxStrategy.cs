using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class InterfaceDeclarationSyntaxStrategy : DictionaryRefactoringStrategy
	{
		protected override IEnumerable<string> IgnorableWords => new List<string> { "I" };
		protected override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> {
			{ "_", new List<string> { "" } }
		};
		protected override string NamePrefix => "I";

		protected override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((InterfaceDeclarationSyntax)syntaxNode).Identifier;
		}
	}
}
