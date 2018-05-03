using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.UseOfVar
{
    public sealed class UseOfVarRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.UseOfVar.GetDiagnosticId();
        public string Title => RefactoringMessageFactory.UseOfVarTitle();
        public string Description => RefactoringMessageFactory.UseOfVarDescription();

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.LocalDeclarationStatement };

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            return GetFixableNodes(node) == null
                ? DiagnosticInfo.CreateSuccessfulResult()
                : CreateFailedDiagnosticResult(node as LocalDeclarationStatementSyntax);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var variableNode = (LocalDeclarationStatementSyntax)node;
            var declaration = variableNode.Declaration;

            if (!CanUseVarKeyword(variableNode, declaration))
                return null;
            
            return new[]
            {
                variableNode.WithDeclaration(declaration.WithType(VarType()))
                    .NormalizeWhitespace()
            };
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<LocalDeclarationStatementSyntax>(token);

        private static bool CanUseVarKeyword(LocalDeclarationStatementSyntax variableNode,
            VariableDeclarationSyntax declaration)
        {
            var declarationType = GetDeclarationType(declaration);

            if (IsConstVariable(variableNode) || declarationType == null ||
                declaration.Variables.Count != 1 || declarationType.IsValueType)
                return false;

            var variable = declaration.Variables.First();
            return variable.Initializer != null && !declaration.Type.IsVar;
        }

        private static ITypeSymbol GetDeclarationType(VariableDeclarationSyntax declaration)
        {
            var semanticModel = GetSemanticModel(declaration);
            return (ITypeSymbol)semanticModel.GetSymbolInfo(declaration.Type).Symbol;
        }

        private static SemanticModel GetSemanticModel(VariableDeclarationSyntax declaration)
        {
            var classNode = SyntaxNodeHelper.FindAncestorOfType<ClassDeclarationSyntax>(declaration.Type.GetFirstToken());
            return SemanticSymbolBuilder.GetSemanticModel(classNode);
        }

        public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
            GetReplaceableNode(token);
        
        private static TypeSyntax VarType() => SyntaxFactory.ParseTypeName("var");

        private static bool IsConstVariable(LocalDeclarationStatementSyntax variableNode) =>
            variableNode.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.ConstKeyword);

        private static DiagnosticInfo CreateFailedDiagnosticResult(LocalDeclarationStatementSyntax variableNode) =>
            DiagnosticInfo.CreateFailedResult(CreateFixMessage(variableNode), null, GetMarkableLocation(variableNode));

        private static Location GetMarkableLocation(LocalDeclarationStatementSyntax variableNode) =>
            variableNode.Declaration.Type.GetLocation();

        private static string CreateFixMessage(LocalDeclarationStatementSyntax variableNode) =>
            RefactoringMessageFactory.UseOfVarMessage(SyntaxNodeHelper.GetText(variableNode.Declaration.Type));
    }
}
