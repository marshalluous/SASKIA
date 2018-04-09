using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.MethodPropertyIdentifierConvention
{
    public sealed class MethodPropertyIdentifierConventionRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.MethodPropertyIdentifierConvention.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => DiagnosticId;


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            if (GetFixableNodes(node) == null)
                return DiagnosticInfo.CreateSuccessfulResult();
            return DiagnosticInfo.CreateFailedResult("sadfkjl");
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var identifierToken = GetIdentifierToken(node);
            var newIdentifierText = IdentifierChecker.ToUpperCamelCaseIdentifier(identifierToken.Text);

            if (identifierToken.Text == newIdentifierText)
                return null;

            return new[] { node.ReplaceToken(identifierToken, SyntaxFactory.Identifier(newIdentifierText)) };
        }

        private static SyntaxToken GetIdentifierToken(SyntaxNode node)
        {
            if (node is MethodDeclarationSyntax methodNode)
                return methodNode.Identifier;

            if (node is PropertyDeclarationSyntax propertyNode)
                return propertyNode.Identifier;

            return default(SyntaxToken);
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorWithPredicate(token,
                node => node is MethodDeclarationSyntax || node is PropertyDeclarationSyntax);
        
        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.MethodDeclaration, SyntaxKind.PropertyDeclaration };
    }
} 