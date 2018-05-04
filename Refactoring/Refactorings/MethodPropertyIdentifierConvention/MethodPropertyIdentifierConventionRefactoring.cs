using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.MethodPropertyIdentifierConvention
{
    public sealed class MethodPropertyIdentifierConventionRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.MethodPropertyIdentifierConvention.GetDiagnosticId();
        public string Title => RefactoringMessages.MethodPropertyIdentifierConventionTitle();
        public string Description => RefactoringMessages.MethodPropertyIdentifierConventionDescription();
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
		    return GetFixableNodes(node, out var newIdentifierText) == null ? 
		        DiagnosticInfo.CreateSuccessfulResult() : 
		        DiagnosticInfo.CreateFailedResult(RefactoringMessages.MethodPropertyIdentifierConventionMessage(newIdentifierText), null, GetIdentifierToken(node).GetLocation());
		}

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            return GetFixableNodes(node, out _);
        }

        private static IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node, out string newIdentifierText)
        {
            var identifierToken = GetIdentifierToken(node);
            newIdentifierText = IdentifierChecker.ToUpperCamelCaseIdentifier(identifierToken.Text);
            return identifierToken.Text == newIdentifierText ?
                null :
                new[] { node.ReplaceToken(identifierToken, SyntaxFactory.Identifier(newIdentifierText)) };
        }

        private static SyntaxToken GetIdentifierToken(SyntaxNode node)
        {
            switch (node)
            {
                case MethodDeclarationSyntax methodNode:
                    return methodNode.Identifier;
                case PropertyDeclarationSyntax propertyNode:
                    return propertyNode.Identifier;
            }

            return default(SyntaxToken);
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorWithPredicate(token,
                node => node is MethodDeclarationSyntax || node is PropertyDeclarationSyntax);
        
        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration };
    }
} 