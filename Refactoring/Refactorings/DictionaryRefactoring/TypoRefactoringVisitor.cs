using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.DictionaryRefactoring
{
	internal sealed class TypoRefactoringVisitor : CSharpSyntaxRewriter
	{
		private readonly SemanticModel semanticModel;
		private readonly ISymbol memberSymbol;

		public TypoRefactoringVisitor(SemanticModel semanticModel, ISymbol memberSymbol)
		{
			this.semanticModel = semanticModel;
			this.memberSymbol = memberSymbol;
		}
		
		public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax identifierNode)
		{
			var identifierSymbol = semanticModel.GetSymbolInfo(identifierNode);

			if (identifierSymbol.Symbol == memberSymbol)
			{
				return SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("hallo"));
			}

			return base.VisitIdentifierName(identifierNode);
		}
	}
}
