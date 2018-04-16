using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using System.Linq;

namespace Refactoring.Refactorings.IllegalFieldAccess
{
    public sealed class IllegalFieldAccessRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.IllegalFieldAccess.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => Title;
        
        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node) =>
            new[] {node};


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            return GetFixableNodes(node) == null
                ? DiagnosticInfo.CreateSuccessfulResult()
                : DiagnosticInfo.CreateFailedResult("xxxx");
        }

		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
		{
            var fieldNode = (FieldDeclarationSyntax) node;

            if (HasOnlyPrivateModifier(fieldNode))
                yield break;

            var nonVisibilityModifiers = GetNonVisibilityModifiers(fieldNode);
            
            yield return fieldNode
                .WithModifiers(new SyntaxTokenList(new[] { SyntaxFactory.Token(SyntaxKind.PrivateKeyword) }))
                .AddModifiers(nonVisibilityModifiers.ToArray())
                .NormalizeWhitespace();
		}

        private static IEnumerable<string> GetVisibilityModifiers()
        {
            return new[] { "private", "public", "protected", "internal" };
        }

        private static bool IsVisibilityModifier(string modifier)
        {
            return GetVisibilityModifiers().Contains(modifier);
        }

        private static IEnumerable<SyntaxToken> GetNonVisibilityModifiers(FieldDeclarationSyntax fieldNode)
        {
            return fieldNode.Modifiers.Where(modifier => !IsVisibilityModifier(modifier.Text));
        }

        private static bool HasOnlyPrivateModifier(BaseFieldDeclarationSyntax fieldNode)
        {
            var presentModifiers = fieldNode.Modifiers.Where(modifier => IsVisibilityModifier(modifier.Text.Trim())).ToArray();
            return presentModifiers.Length == 1 && presentModifiers.First().Text == "private";
        }
        
        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<FieldDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.FieldDeclaration};
    }
}
