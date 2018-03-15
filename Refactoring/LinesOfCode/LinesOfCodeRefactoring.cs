using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring
{
    public sealed class LinesOfCodeRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA100";
        public string Title => DiagnosticId;
        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.ClassDeclaration, SyntaxKind.MethodDeclaration};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax classNode)
            {
                if (CountLines(node) > 100)
                    return DiagnosticInfo.CreateFailedResult($"Class {classNode.Identifier.Text} is too long");
            }
            else if (node is MethodDeclarationSyntax methodNode)
            {
                if (CountLines(node) > 15)
                    return DiagnosticInfo.CreateFailedResult($"Method {methodNode.Identifier.Text} is too long");
            }

            return DiagnosticInfo.CreateSuccessfulResult();
        }

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            throw new NotImplementedException();
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            throw new NotImplementedException();
        }

        private static int CountLines(SyntaxNode syntaxNode)
        {
            return SyntaxNodeHelper
                .GetText(syntaxNode)
                .Count(character => character == '\n');
        }
    }
}