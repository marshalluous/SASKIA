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

            var classNode = (ClassDeclarationSyntax)node;
            var semanticModel = SemanticSymbolBuilder.GetSemanticModel(classNode);

            var methodNodeList = classNode.Members.OfType<MethodDeclarationSyntax>().ToList();
            var fieldSymbolList = GetFieldSymbolList(semanticModel, classNode);

            var fieldAccessCounterMap = CountMethodsFieldAccesses(semanticModel, methodNodeList, fieldSymbolList);
            var lackOfCohesionValue = CalculateLackOfCohesionValue(fieldSymbolList, methodNodeList, fieldAccessCounterMap);

            return lackOfCohesionValue > lackOfCohesionThreshold ?
                DiagnosticInfo.CreateFailedResult("Lack Of Cohesion!", lackOfCohesionValue, classNode.Identifier.GetLocation()) :
                DiagnosticInfo.CreateSuccessfulResult(lackOfCohesionValue);
        }

        private static Dictionary<IFieldSymbol, int> CountMethodsFieldAccesses(SemanticModel semanticModel,
            IEnumerable<MethodDeclarationSyntax> methodNodeList,
            IEnumerable<IFieldSymbol> fieldSymbolList)
        {
            var counterVisitor = new LackOfCohesionCounterVisitor(semanticModel);
            var fieldAccessCounterMap = fieldSymbolList.ToDictionary(field => field, _ => 0);

            foreach (var method in methodNodeList)
            {
                var visitedFields = counterVisitor.Visit(method);

                foreach (var visitedField in visitedFields)
                {
                    ++fieldAccessCounterMap[visitedField];
                }
            }

            return fieldAccessCounterMap;
        }

        private static double CalculateLackOfCohesionValue(IEnumerable<IFieldSymbol> fieldSymbolList, IReadOnlyCollection<MethodDeclarationSyntax> methodNodeList, Dictionary<IFieldSymbol, int> fieldAccessCounterMap)
        {
            var averageFieldAccessCount = fieldAccessCounterMap.Values.Sum() / (double)fieldSymbolList.Count();
            var numberOfMethods = methodNodeList.Count;
            return numberOfMethods < 2 ? 0d : (numberOfMethods - averageFieldAccessCount) /
                                                 (numberOfMethods - 1);
        }

        private static List<IFieldSymbol> GetFieldSymbolList(SemanticModel semanticModel, BaseTypeDeclarationSyntax typeNode)
        {
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeNode);
            return typeSymbol.GetMembers().OfType<IFieldSymbol>().ToList();
        }
        
        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<ClassDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.ClassDeclaration};
    }
}