using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.WhitespaceFix
{
    public sealed class WhitespaceFixRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.WhitespaceFix.GetDiagnosticId();
        public string Title => RefactoringMessages.WhitespaceFixTitle();
        public string Description => RefactoringMessages.WhitespaceFixDescription();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var namespaceNode = (NamespaceDeclarationSyntax) node;
            return GetFixableNodes(node) == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                GetFailedDiagnosticInfo(namespaceNode);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var whitespaceFixNode = GetFixedNode(node);
            return node.ToString() == whitespaceFixNode.ToString() ?
                null :
                new[] { whitespaceFixNode };
        }
        
        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<NamespaceDeclarationSyntax>(token);
        
        public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
            GetReplaceableNode(token);
    
        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.NamespaceDeclaration };

        private static SyntaxNode GetFixedNode(SyntaxNode node) =>
            node.NormalizeWhitespace().WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed);

        private static DiagnosticInfo GetFailedDiagnosticInfo(NamespaceDeclarationSyntax namespaceNode) =>
            DiagnosticInfo.CreateFailedResult(RefactoringMessages.WhitespaceFixMessage()
                , null, GetLocation(namespaceNode));

        private static Location GetLocation(NamespaceDeclarationSyntax node) =>
            node.Name.GetLocation();
    }
}