﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.PotentialStaticMethod
{
    public sealed class PotentialStaticMethodRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.PotentialStaticMethod.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => Title;
        
        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax)node;
            var classNode = (ClassDeclarationSyntax)node.Parent;
            var semanticModel = GetSemanticModel(classNode);

            if (MethodIsStatic(methodNode) || !MethodIsPrivate(methodNode))
                return null;

            var classSymbol = semanticModel.GetDeclaredSymbol(classNode);
            var isPotentialStaticMethod = IsPotentialStaticMethod(methodNode.Body, semanticModel, classSymbol);

            if (isPotentialStaticMethod)
            {
                var staticKeyword = SyntaxFactory.Token(SyntaxKind.StaticKeyword);
                return new[] { methodNode.AddModifiers(staticKeyword).NormalizeWhitespace() };
            }

            return null;
        }
        
        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var methodNode = (MethodDeclarationSyntax)node;
            var fixedNode = GetFixableNodes(node);

            return fixedNode == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult("Method can be static", null, methodNode.Identifier.GetLocation());
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<MethodDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.MethodDeclaration };

        private static bool IsPotentialStaticMethod(SyntaxNode syntaxNode, SemanticModel semanticModel, ITypeSymbol classSymbol)
        {
            var nodeText = SyntaxNodeHelper.GetText(syntaxNode);

            if (nodeText == "this" || nodeText == "base")
                return false;


            if (syntaxNode is IdentifierNameSyntax)
            {
                var declaredSymbol = semanticModel.GetSymbolInfo(syntaxNode).Symbol;

                if (declaredSymbol == null)
                    return true;

                if (declaredSymbol.ContainingType != classSymbol)
                    return true;

                if (!(declaredSymbol is IFieldSymbol || declaredSymbol is IMethodSymbol || declaredSymbol is IPropertySymbol))
                    return true;

                return declaredSymbol.IsStatic;
            }

            return syntaxNode.ChildNodes().All(child => IsPotentialStaticMethod(child, semanticModel, classSymbol));
        }

        private static bool MethodIsPrivate(MethodDeclarationSyntax methodNode)
        {
            return methodNode.Modifiers.Any(SyntaxKind.PrivateKeyword);
        }

        private static bool MethodIsStatic(MethodDeclarationSyntax methodNode)
        {
            return methodNode.Modifiers.Any(SyntaxKind.StaticKeyword);
        }

        private static SemanticModel GetSemanticModel(BaseTypeDeclarationSyntax classNode)
        {
            var classSyntaxTree = classNode.SyntaxTree;
            var compilation = CSharpCompilation.Create("CompilationUnit", new[] { classSyntaxTree });
            return compilation.GetSemanticModel(classNode.SyntaxTree);
        }
    }
}