using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.LongParameterList
{
    public sealed class LongParameterListRefactoring : IRefactoring
    {
        private const int ParameterCountThreshold = 3;

        public string DiagnosticId => RefactoringId.LongParameterList.GetDiagnosticId();

        public string Title => RefactoringMessageFactory.LongParameterListTitle();

        public string Description => RefactoringMessageFactory.LongParameterListDescription();
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);
        
        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) =>
            null;

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax)node;
            var parameterCount = GetParameterCount(methodNode);
            return parameterCount <= ParameterCountThreshold ? 
                DiagnosticInfo.CreateSuccessfulResult(parameterCount) : 
                CreateFailedDiagnostics(parameterCount, methodNode.Identifier.GetLocation());
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<MethodDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.MethodDeclaration };

        private static DiagnosticInfo CreateFailedDiagnostics(int parameterCount, Location markableLocation)
        {
            var diagnosticMessage = RefactoringMessageFactory.LongParameterListMessage(parameterCount);
            return DiagnosticInfo.CreateFailedResult(diagnosticMessage, parameterCount, markableLocation);
        }

        private static int GetParameterCount(BaseMethodDeclarationSyntax methodNode) =>
            methodNode.ParameterList
                .ChildNodes().OfType<ParameterSyntax>()
                .Count(parameter => parameter.Modifiers.All(modifier => modifier.Text != "params"));
    }
}