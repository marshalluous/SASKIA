using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using System.Linq;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.IllegalFieldAccess
{
    public sealed class IllegalFieldAccessRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.IllegalFieldAccess.GetDiagnosticId();
        public string Title => RefactoringMessages.IllegalFieldAccessTitle();
        public string Description => RefactoringMessages.IllegalFieldAccessDescription();
        
        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node) =>
            new[] {node};
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            return GetFixableNodes(node) == null
                ? DiagnosticInfo.CreateSuccessfulResult()
                : DiagnosticInfo.CreateFailedResult(RefactoringMessages.IllegalFieldAccessMessage());
        }

		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
		{
            var fieldNode = (FieldDeclarationSyntax) node;

		    if (HasOnlyPrivateModifier(fieldNode))
		        return null;

            var nonVisibilityModifiers = GetNonVisibilityModifiers(fieldNode);
            
            return new [] { fieldNode
                .WithModifiers(new SyntaxTokenList(new[] { SyntaxFactory.Token(SyntaxKind.PrivateKeyword) }))
                .AddModifiers(nonVisibilityModifiers.ToArray())
                .NormalizeWhitespace()
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
            };
		}

        private static IEnumerable<string> GetVisibilityModifiers() =>
            new[] { "private", "public", "protected", "internal" };

        private static bool IsVisibilityModifier(string modifier) =>
            GetVisibilityModifiers().Contains(modifier);
    
        private static IEnumerable<SyntaxToken> GetNonVisibilityModifiers(BaseFieldDeclarationSyntax fieldNode)
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
