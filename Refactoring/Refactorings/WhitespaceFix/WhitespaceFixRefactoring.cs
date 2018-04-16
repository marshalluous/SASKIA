using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.WhitespaceFix
{
    public sealed class WhitespaceFixRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.WhitespaceFix.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => DiagnosticId;

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var namespaceNode = (NamespaceDeclarationSyntax) node;

            return GetFixableNodes(node) == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult("fix whitespace", null, namespaceNode.Name.GetLocation());
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var whitespaceFixNode = node.NormalizeWhitespace();
            return node.ToString() == whitespaceFixNode.ToString() ?
                null : 
                new[] { whitespaceFixNode };
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return token.Parent;
        }

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.NamespaceDeclaration };
    }
}
