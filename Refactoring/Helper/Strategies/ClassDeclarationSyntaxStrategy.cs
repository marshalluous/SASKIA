using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class ClassDeclarationSyntaxStrategy : TypoRefactoringStrategy
	{
		protected override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((ClassDeclarationSyntax)syntaxNode).Identifier;
		}

		protected override void RemoveIgnorableWords(List<string> wordList)
		{
		}
	}
}
