using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Refactoring.Helper;
using System.Collections.Generic;

namespace Refactoring.DictionaryRefactorings
{
	public sealed class TypoRefactoring : IRefactoring
	{
		public string DiagnosticId => "SASKIA200";
		public string Title => DiagnosticId;
		public string Description => "Typo";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration, SyntaxKind.InterfaceDeclaration, SyntaxKind.MethodDeclaration, SyntaxKind.VariableDeclarator, SyntaxKind.PropertyDeclaration };

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
			var strategy = TypoRefactoryFactory.GetStrategy(node.GetType());
			return strategy.Diagnose(node, Description);
		}

		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
			var strategy = TypoRefactoryFactory.GetStrategy(node.GetType());
			return strategy.EvaluateNodes(node);
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}
	}
}
