using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.LinesOfCode
{
    public sealed class LinesOfCodeRefactoring : IRefactoring
    {
        private const int ClassLinesOfCodeThreshold = 100;
        private const int MethodLinesOfCodeThreshold = 15;

        public string DiagnosticId => RefactoringId.LinesOfCode.GetDiagnosticId();
        public string Title => DiagnosticId;
        public string Description => Title;
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.ClassDeclaration, SyntaxKind.MethodDeclaration};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            switch (node)
            {
                case ClassDeclarationSyntax classNode:
                    return CheckNodesLinesOfCode(node, classNode.Identifier, "Class", ClassLinesOfCodeThreshold);

                case MethodDeclarationSyntax methodNode:
                    return CheckNodesLinesOfCode(node, methodNode.Identifier, "Method", MethodLinesOfCodeThreshold);
            }

            return DiagnosticInfo.CreateSuccessfulResult();
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) =>
            new[] { node };

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            token.Parent;

        private static DiagnosticInfo CheckNodesLinesOfCode(SyntaxNode node, SyntaxToken identifier, string nodeType, int threshold)
        {
            var linesOfCode = CountLines(node);
            return linesOfCode > threshold ? 
                DiagnosticInfo.CreateFailedResult($"{nodeType} {identifier.Text} is too long") :
                DiagnosticInfo.CreateSuccessfulResult();
        }

        private static int CountLines(SyntaxNode syntaxNode) => 
            SyntaxNodeHelper.GetText(syntaxNode)
                .Count(character => character == '\n');
    }
}