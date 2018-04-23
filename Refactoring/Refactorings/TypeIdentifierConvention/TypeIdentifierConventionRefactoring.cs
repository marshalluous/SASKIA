using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Refactoring.Refactorings.TypeIdentifierConvention
{
    public sealed class TypeIdentifierConventionRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.TypeIdentifierConvention.GetDiagnosticId();
        public string Title => RefactoringMessageFactory.TypeIdentifierTitle();
        public string Description => RefactoringMessageFactory.TypeIdentifierDescription();
        public SyntaxNode GetReplaceableNode(SyntaxToken token) => token.Parent;
        public SyntaxNode GetReplaceableRootNode(SyntaxToken token) => token.Parent;
		
		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
		    var fixableNodes = GetFixableNodes(node);
            return fixableNodes == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory
                    .TypeIdentifierMessage(GetIdentifierText(node), GetIdentifierText(fixableNodes.First())), null, GetIdentifierToken(node).GetLocation());
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var identifier = GetIdentifierToken(node);
            
            switch (node)
            {
                case InterfaceDeclarationSyntax _:
                    return RefactorInterfaceTypeName(node, identifier);

                case DelegateDeclarationSyntax _:
                case BaseTypeDeclarationSyntax _:
                    return RefactorNonInterfaceTypeName(node, identifier);

                default:
                    return null;
            }
        }

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration, SyntaxKind.EnumDeclaration,
                SyntaxKind.InterfaceDeclaration, SyntaxKind.DelegateDeclaration };

        private static IEnumerable<SyntaxNode> RefactorInterfaceTypeName(SyntaxNode node, SyntaxToken identifier)
        {
            if (CheckInterfacePrefix(identifier.Text))
                return null;

            var newIdentifier = FixInterfaceName(identifier.Text);
            var newIdentifierToken = SyntaxFactory.Identifier(newIdentifier);
            return new[] { node.ReplaceToken(identifier, newIdentifierToken) };
        }

        private static IEnumerable<SyntaxNode> RefactorNonInterfaceTypeName(SyntaxNode node, SyntaxToken identifier)
        {
            if (IdentifierChecker.IsUpperCamelCase(identifier.Text))
                return null;

            var newIdentifierToken =
                SyntaxFactory.Identifier(IdentifierChecker.ToUpperCamelCaseIdentifier(identifier.Text));
            return new[] {node.ReplaceToken(identifier, newIdentifierToken)};
        }

        private static string FixInterfaceName(string identifierText)
        {
            if (!CheckInterfacePrefix(identifierText))
                return "I" + IdentifierChecker.ToUpperCamelCaseIdentifier(identifierText);
            return identifierText;
        }

        private static bool CheckInterfacePrefix(string identifierText)
        {
            return IdentifierChecker.IsUpperCamelCase(identifierText) &&
                   identifierText.StartsWith("I");
        }

        private static string GetIdentifierText(SyntaxNode node)
        {
            return GetIdentifierToken(node).Text;
        }

        private static SyntaxToken GetIdentifierToken(SyntaxNode node)
        {
            switch (node)
            {
                case BaseTypeDeclarationSyntax typeNode:
                    return typeNode.Identifier;

                case DelegateDeclarationSyntax delegateNode:
                    return delegateNode.Identifier;

                default:
                    return default(SyntaxToken);
            }
        }
    }
}