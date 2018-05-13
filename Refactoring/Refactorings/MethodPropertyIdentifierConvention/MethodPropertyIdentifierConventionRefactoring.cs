using System.Collections.Generic;
using System.Linq;
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

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorWithPredicate(token,
                node => node is MethodDeclarationSyntax || node is PropertyDeclarationSyntax);
        
        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration };

        private static IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node, out string newIdentifierText)
        {
            var identifierToken = GetIdentifierToken(node);
            
            if (IsExternMethod(node))
            {
                newIdentifierText = identifierToken.Text;
                return null;
            }

            newIdentifierText = IdentifierChecker.ToUpperCamelCaseIdentifier(identifierToken.Text);
            return identifierToken.Text == newIdentifierText
                ? null
                : new[] {node.ReplaceToken(identifierToken, SyntaxFactory.Identifier(newIdentifierText))};
        }

        private static bool IsExternMethod(SyntaxNode node) => 
            node is MethodDeclarationSyntax methodNode && methodNode.Modifiers.Any(IsExternModifier);

        private static bool IsExternModifier(SyntaxToken token) =>
            token.Text == "extern";

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
    }
}