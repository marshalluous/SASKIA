using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Refactoring.DictionaryRefactorings
{
    public sealed class ClassNameNounRefactoring : IRefactoring
	{
		public string DiagnosticId => "SASKIA220";
		public string Title => DiagnosticId;
		public string Description => "Class name must be a noun";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration, SyntaxKind.InterfaceDeclaration, SyntaxKind.EnumDeclaration, SyntaxKind.StructDeclaration };


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
			var strategy = TypoRefactoryFactory.GetStrategy(node.GetType());
			return strategy.DiagnoseNoun(node, Description);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
		{
            return new[] { node };
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}
	}
}
