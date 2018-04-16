using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.TypeIdentifierConvention
{
    public sealed class TypeIdentifierConventionRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.TypeIdentifierConvention.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => Title;

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            return GetFixableNodes(node) == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult("Unconventional type name");
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var identifierText = GetIdentifier(node);
            
            if (node is InterfaceDeclarationSyntax interfaceNode)
            {
                if (!CheckInterfacePrefix(identifierText))
                {
                    var newIdentifier = FixInterfaceName(identifierText);
                    var newIdentifierToken = SyntaxFactory.Identifier(newIdentifier);
                    return new[] { node.ReplaceToken(interfaceNode.Identifier, newIdentifierToken) };
                }
            }

            if (node is DelegateDeclarationSyntax delegateNode)
            {
                if (!IdentifierChecker.IsUpperCamelCase(identifierText))
                {
                    var newIdentifierToken = SyntaxFactory.Identifier(IdentifierChecker.ToUpperCamelCaseIdentifier(identifierText));
                    return new[] { node.ReplaceToken(delegateNode.Identifier, newIdentifierToken) };
                }
            }
            
            if (node is BaseTypeDeclarationSyntax typeNode)
            {
                if (IdentifierChecker.IsUpperCamelCase(identifierText))
                    return null;

                var newIdentifierToken = SyntaxFactory.Identifier(IdentifierChecker.ToUpperCamelCaseIdentifier(identifierText));
                return new[] { node.ReplaceToken(typeNode.Identifier, newIdentifierToken) };
            }

            return null;
        }

        private string FixInterfaceName(string identifierText)
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

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return token.Parent;
        }

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration, SyntaxKind.EnumDeclaration,
                SyntaxKind.InterfaceDeclaration, SyntaxKind.DelegateDeclaration };

        private static string GetIdentifier(SyntaxNode node)
        {
            switch (node)
            {
                case BaseTypeDeclarationSyntax typeNode:
                    return typeNode.Identifier.Text.Trim();

                case DelegateDeclarationSyntax delegateNode:
                    return delegateNode.Identifier.Text.Trim();
            }

            return string.Empty;
        }
    }
}