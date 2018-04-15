using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class InterfaceDeclarationSyntaxStrategy : TypoRefactoringStrategy
	{
		protected override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((InterfaceDeclarationSyntax)syntaxNode).Identifier;
		}

		protected override void RemoveIgnorableWords(List<string> wordList)
		{
			wordList.Remove("I");
		}
	}
}
