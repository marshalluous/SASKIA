using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Refactoring.Helper;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies;

namespace Refactoring.Refactorings.DictionaryRefactoring
{
	public sealed class WordTypeRefactoring : IRefactoring
	{
		public string DiagnosticId => RefactoringId.WordTypeChecker.GetDiagnosticId();
        public string Title => DiagnosticId;
		public string Description => "Word Type error";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration, SyntaxKind.InterfaceDeclaration, SyntaxKind.EnumDeclaration, SyntaxKind.StructDeclaration, SyntaxKind.MethodDeclaration };


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
			var strategy = DictionaryRefactoringFactory.GetStrategy(node.GetType(), typeof(WordTypeRefactoringStrategy));
			return strategy.Diagnose(node, Description);
		}

		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
		{
			var strategy = DictionaryRefactoringFactory.GetStrategy(node.GetType(), typeof(WordTypeRefactoringStrategy));
			return strategy.EvaluateNodes(node);
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}
	}
}