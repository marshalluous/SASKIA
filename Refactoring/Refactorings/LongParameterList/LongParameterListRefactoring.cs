using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.LongParameterList
{
    public sealed class LongParameterListRefactoring : IRefactoring
    {
        private const int ParameterCountThreshold = 3;

        public string DiagnosticId => RefactoringId.LongParameterList.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => Title;


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);


		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            return null;
        }

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax)node;
            var parameterCount = methodNode.ParameterList.ChildNodes().Count();

            if (parameterCount > ParameterCountThreshold)
            {
                return DiagnosticInfo.CreateFailedResult("Long Parameter List", parameterCount, methodNode.Identifier.GetLocation());
            }

            return DiagnosticInfo.CreateSuccessfulResult(parameterCount);
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<MethodDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.MethodDeclaration };
    }
}
