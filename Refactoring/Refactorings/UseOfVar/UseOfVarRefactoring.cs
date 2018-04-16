using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.UseOfVar
{
    public sealed class UseOfVarRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.UseOfVar.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.LocalDeclarationStatement};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var variableNode = (LocalDeclarationStatementSyntax) node;

            return GetFixableNodes(node) == null
                ? DiagnosticInfo.CreateSuccessfulResult()
                : DiagnosticInfo.CreateFailedResult("use var", null, variableNode.Declaration.Type.GetLocation());
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var variableNode = (LocalDeclarationStatementSyntax) node;

            var declaration = variableNode.Declaration;

            if (CanUseVarKeyword(declaration))
                return null;

            return new[]
            {
                variableNode.WithDeclaration(declaration.WithType(VarType())).NormalizeWhitespace()
            };
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<LocalDeclarationStatementSyntax>(token);
        
        private static TypeSyntax VarType()
        {
            return SyntaxFactory.ParseTypeName("var");
        }

        private static bool CanUseVarKeyword(VariableDeclarationSyntax declaration)
        {
            if (declaration.Variables.Count != 1)
                return true;

            var variable = declaration.Variables.First();
            return variable.Initializer == null || declaration.Type.IsVar;
        }

        private static SemanticModel CreateSemanticModel(BaseTypeDeclarationSyntax classNode)
        {
            var classSyntaxTree = classNode.SyntaxTree;
            var compilation = CSharpCompilation.Create("CompilationUnit", new[] { classSyntaxTree });
            var model = compilation.GetSemanticModel(classNode.SyntaxTree);
            return model;
        }

        private static ITypeSymbol GetTypeSymbol(BaseTypeDeclarationSyntax classNode)
        {
            var model = CreateSemanticModel(classNode);
            var classSemanticNode = classNode.SyntaxTree.GetRoot().DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(node => node.Identifier == classNode.Identifier);

            return model.GetDeclaredSymbol(classSemanticNode);
        }

        public SyntaxNode GetReplaceableRootNode(SyntaxToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
