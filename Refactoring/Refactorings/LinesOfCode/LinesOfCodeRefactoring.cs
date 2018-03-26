using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.LinesOfCode
{
    public sealed class LinesOfCodeRefactoring : IRefactoring
    {
        private const int ClassLinesOfCodeThreshold = 100;
        private const int MethodLinesOfCodeThreshold = 15;

        public string DiagnosticId => "SASKIA100";
        public string Title => DiagnosticId;
        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.ClassDeclaration, SyntaxKind.MethodDeclaration};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax classNode)
            {
                if (CountLines(node) > ClassLinesOfCodeThreshold)
                    return DiagnosticInfo.CreateFailedResult($"Class {classNode.Identifier.Text} is too long");
            }
            else if (node is MethodDeclarationSyntax methodNode)
            {
                if (CountLines(node) > MethodLinesOfCodeThreshold)
                    return DiagnosticInfo.CreateFailedResult($"Method {methodNode.Identifier.Text} is too long");
            }

            return DiagnosticInfo.CreateSuccessfulResult();
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            yield return node;
        }
    
        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return token.Parent; 
        }

        private static int CountLines(SyntaxNode syntaxNode)
        {
            return SyntaxNodeHelper
                .GetText(syntaxNode)
                .Count(character => character == '\n');
        }
    }
}