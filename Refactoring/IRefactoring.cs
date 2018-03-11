using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace Refactoring
{
    public interface IRefactoring
    {
        IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize();
        string DiagnosticId { get; }
        string Title { get; }
        string Description { get; }
        DiagnosticInfo DoDiagnosis(SyntaxNode node);
        IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node);
        SyntaxNode GetReplaceableNode(SyntaxToken token);
    }
}