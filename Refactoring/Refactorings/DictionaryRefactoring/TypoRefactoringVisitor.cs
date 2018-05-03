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

			return identifierSymbol.Symbol == memberSymbol ? 
			    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("hallo")) : base.VisitIdentifierName(identifierNode);
		}
	}
}
