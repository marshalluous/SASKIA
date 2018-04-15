using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class VariableDeclaratorSyntaxStrategy : TypoRefactoringStrategy
	{
		protected override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((VariableDeclaratorSyntax)syntaxNode).Identifier;
		}

		protected override void RemoveIgnorableWords(List<string> wordList)
		{
			wordList.Remove("_");
		}
	}
}
