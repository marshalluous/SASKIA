using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.PotentialStaticMethod
{
    public sealed class PotentialStaticMethodRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.PotentialStaticMethod.GetDiagnosticId();
        public string Title => RefactoringMessages.PotentialStaticMethodTitle();
        public string Description => RefactoringMessages.PotentialStaticMethodDescription();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax)node;
            return GetFixableNodes(node) == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                CreateFailedDiagnosticResult(methodNode);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax)node;
            var classNode = (ClassDeclarationSyntax)node.Parent;
            var semanticModel = SemanticSymbolBuilder.GetSemanticModel(classNode);

            if (MethodIsStatic(methodNode) || !MethodIsPrivate(methodNode))
                return null;

            var classSymbol = semanticModel.GetDeclaredSymbol(classNode);
            return IsPotentialStaticMethod(methodNode.Body, semanticModel, classSymbol) ? new[] { StaticMethodNode(methodNode) } : null;
        }

        private static MethodDeclarationSyntax StaticMethodNode(MethodDeclarationSyntax methodNode) => 
            methodNode.AddModifiers(StaticKeyword()).NormalizeWhitespace();

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<MethodDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.MethodDeclaration };

        private static bool IsPotentialStaticMethod(SyntaxNode syntaxNode, SemanticModel semanticModel, ITypeSymbol classSymbol)
        {
            var nodeText = SyntaxNodeHelper.GetText(syntaxNode);
            if (nodeText == "this" || nodeText == "base")
                return false;
            if (!(syntaxNode is IdentifierNameSyntax))
                return syntaxNode.ChildNodes()
                    .All(child => IsPotentialStaticMethod(child, semanticModel, classSymbol));

            var declaredSymbol = semanticModel.GetSymbolInfo(syntaxNode).Symbol;
            return SymbolRefersToStaticDeclarationSymbol(classSymbol, declaredSymbol);
        }

        public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
            GetReplaceableNode(token);

        private static bool SymbolRefersToStaticDeclarationSymbol(ITypeSymbol classSymbol, ISymbol declaredSymbol) => 
            IsDeclarationInOtherClass(classSymbol, declaredSymbol) || !IsClassMemberSymbol(declaredSymbol) || declaredSymbol.IsStatic;

        private static bool IsClassMemberSymbol(ISymbol declaredSymbol) => 
            declaredSymbol is IFieldSymbol || declaredSymbol is IMethodSymbol || declaredSymbol is IPropertySymbol;

        private static bool IsDeclarationInOtherClass(ITypeSymbol classSymbol, ISymbol declaredSymbol) => 
            declaredSymbol == null || declaredSymbol.ContainingType != classSymbol;

        private static DiagnosticInfo CreateFailedDiagnosticResult(MethodDeclarationSyntax methodNode) =>
            DiagnosticInfo.CreateFailedResult(RefactoringMessages.PotentialStaticMethodMessage(), null, methodNode.Identifier.GetLocation());

        private static SyntaxToken StaticKeyword() =>
            SyntaxFactory.Token(SyntaxKind.StaticKeyword);

        private static bool MethodIsPrivate(BaseMethodDeclarationSyntax methodNode) => 
            methodNode.Modifiers.Any(SyntaxKind.PrivateKeyword);

        private static bool MethodIsStatic(BaseMethodDeclarationSyntax methodNode) => 
            methodNode.Modifiers.Any(SyntaxKind.StaticKeyword);
    }
}