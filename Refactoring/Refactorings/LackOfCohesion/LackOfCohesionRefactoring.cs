using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.LackOfCohesion
{
    public sealed class LackOfCohesionRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.LackOfCohesion.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => Title;

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) =>
            new [] { node };


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            const double lackOfCohesionThreshold = 0.6d;

            var classNode = (ClassDeclarationSyntax) node;
            var fieldNodeList = classNode.Members.OfType<FieldDeclarationSyntax>().ToList();
            var methodNodeList = classNode.Members.OfType<MethodDeclarationSyntax>().ToList();
            var fieldAccessCounterMap = fieldNodeList.ToDictionary(field => field, _ => 0);
            
            foreach (var method in methodNodeList)
            {
                var visitedFields = new HashSet<FieldDeclarationSyntax>();
                GetMethodsFieldAccesses(method, fieldNodeList, visitedFields);

                foreach (var visitedField in visitedFields)
                {
                    ++fieldAccessCounterMap[visitedField];
                }
            }

            var averageFieldAccessCount = fieldAccessCounterMap.Values.Sum() / (double) fieldNodeList.Count;
            var numberOfMethods = methodNodeList.Count;

            var lackOfCohesionValue = numberOfMethods < 2 ? 0d : (numberOfMethods - averageFieldAccessCount) / 
                                                 (numberOfMethods - 1);

            return lackOfCohesionValue > lackOfCohesionThreshold ? 
                DiagnosticInfo.CreateFailedResult("LCOM!", lackOfCohesionValue, classNode.Identifier.GetLocation()) :
                DiagnosticInfo.CreateSuccessfulResult(lackOfCohesionValue);
        }

        private static void GetMethodsFieldAccesses(SyntaxNode node, 
            IReadOnlyCollection<FieldDeclarationSyntax> availableFields,
            ISet<FieldDeclarationSyntax> visitedMap)
        {
            if (node is IdentifierNameSyntax identifierNode)
            {
                var accessedFieldName = identifierNode.Identifier.Text;

                var accessedField = availableFields
                    .FirstOrDefault(field => field.Declaration.Variables[0].Identifier.ValueText ==
                    accessedFieldName);

                if (accessedField != null)
                    visitedMap.Add(accessedField);
            }

            foreach (var child in node.ChildNodes())
            {
                GetMethodsFieldAccesses(child, availableFields, visitedMap);
            }
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<ClassDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.ClassDeclaration};
    }
}