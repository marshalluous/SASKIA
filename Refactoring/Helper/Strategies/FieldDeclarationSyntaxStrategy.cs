using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class FieldDeclarationSyntaxStrategy : DictionaryRefactoringStrategy
	{
		protected override IEnumerable<string> IgnorableWords => new List<string>();
		protected override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> { };
		protected override string NamePrefix => "_";

		protected override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((FieldDeclarationSyntax)syntaxNode).Declaration.Variables.First().Identifier;
		}
	}
}
