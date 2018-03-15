using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Refactoring
{
    public interface IRefactoring
    {
        string DiagnosticId { get; }
        string Title { get; }
        string Description { get; }
        IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize();
        DiagnosticInfo DoDiagnosis(SyntaxNode node);
        IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node);
        SyntaxNode GetReplaceableNode(SyntaxToken token);
    }
}