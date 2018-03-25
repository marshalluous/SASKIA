using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.IllegalFieldAccess
{
    public sealed class IllegalFieldAccessRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA777";

        public string Title => DiagnosticId;

        public string Description => Title;

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node) =>
            new[] {node};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var fieldNode = (FieldDeclarationSyntax) node;
            return DiagnosticInfo.CreateSuccessfulResult();
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<FieldDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.FieldDeclaration};
    }
}
