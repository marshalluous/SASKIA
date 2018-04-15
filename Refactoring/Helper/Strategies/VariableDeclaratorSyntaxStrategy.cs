using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class VariableDeclaratorSyntaxStrategy : TypoRefactoringStrategy
	{
		protected override List<string> IgnorableWords => new List<string> { "_" };
		protected override Dictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>>();

		protected override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((VariableDeclaratorSyntax)syntaxNode).Identifier;
		}
	}
}
