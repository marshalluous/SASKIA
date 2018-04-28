using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.Helper.Strategies;
using System.Collections.Generic;

namespace Refactoring.DictionaryRefactorings
{
	public sealed class TypoRefactoring : IRefactoring
	{
		public string DiagnosticId => "SASKIA200";
		public string Title => DiagnosticId;
		public string Description => "Typo in name";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration, SyntaxKind.InterfaceDeclaration, SyntaxKind.MethodDeclaration,
				SyntaxKind.VariableDeclarator, SyntaxKind.PropertyDeclaration, SyntaxKind.FieldDeclaration };

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
			var strategy = DictionaryRefactoryFactory.GetStrategy(node.GetType(), typeof(TypoRefactoringStrategy));
			return strategy.Diagnose(node, Description);
		}

		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
			var strategy = DictionaryRefactoryFactory.GetStrategy(node.GetType(), typeof(TypoRefactoringStrategy));
			return strategy.EvaluateNodes(node);
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}

		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			SyntaxNodeHelper.FindAncestorOfType<ClassDeclarationSyntax>(token);
	}
}
