using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;
using System.Collections.Generic;
using System.Linq;

namespace Refactoring.Refactorings.LackOfCohesion
{
    public sealed class LackOfCohesionRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.LackOfCohesion.GetDiagnosticId();
        public string Title => RefactoringMessages.LackOfCohesionTitle();
        public string Description => Title;

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) =>
            new [] { node };
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            const double lackOfCohesionThreshold = 0.9d;

            var classNode = (ClassDeclarationSyntax) node;
            var semanticModel = SemanticSymbolBuilder.GetSemanticModel(classNode);

            var methodNodeList = classNode.Members
                .OfType<MethodDeclarationSyntax>()
                .Where(methodNode => !IsStatic(methodNode))
                .Where(methodNode => !IsAbstract(methodNode))
                .ToList();

            var fieldSymbolList = GetFieldSymbolList(semanticModel, classNode);

            var fieldAccessCounterMap = CountMethodsFieldAccesses(semanticModel, methodNodeList, fieldSymbolList);
            var lackOfCohesionValue = CalculateLackOfCohesionValue(fieldSymbolList, methodNodeList, fieldAccessCounterMap);

            return lackOfCohesionValue > lackOfCohesionThreshold ?
                CreateFailedDiagnosticResult(classNode, lackOfCohesionValue) :
                DiagnosticInfo.CreateSuccessfulResult(lackOfCohesionValue);
        }
        
        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<ClassDeclarationSyntax>(token);

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] { SyntaxKind.ClassDeclaration };

        private static bool IsStatic(BaseMethodDeclarationSyntax methodNode) =>
            HasModifier(methodNode, "static");

        private static bool IsAbstract(BaseMethodDeclarationSyntax methodNode) =>
            HasModifier(methodNode, "abstract");

        private static bool HasModifier(BaseMethodDeclarationSyntax methodNode, string modifierText) =>
            methodNode.Modifiers.Any(modifier => modifier.Text == modifierText);

        private static DiagnosticInfo CreateFailedDiagnosticResult(BaseTypeDeclarationSyntax classNode, double lackOfCohesionValue) => 
            DiagnosticInfo.CreateFailedResult(RefactoringMessages.LackOfCohesionMessage(lackOfCohesionValue), lackOfCohesionValue, classNode.Identifier.GetLocation());

        private static Dictionary<IFieldSymbol, int> CountMethodsFieldAccesses(SemanticModel semanticModel,
            IEnumerable<MethodDeclarationSyntax> methodNodeList,
            IEnumerable<IFieldSymbol> fieldSymbolList)
        {
            var counterVisitor = new LackOfCohesionCounterVisitor(semanticModel);
            var fieldAccessCounterMap = fieldSymbolList.ToDictionary(field => field, _ => 0);

            foreach (var method in methodNodeList)
            {
                foreach (var visitedField in counterVisitor.Visit(method))
                {
                    if (fieldAccessCounterMap.ContainsKey(visitedField))
                        ++fieldAccessCounterMap[visitedField];
                }
            }

            return fieldAccessCounterMap;
        }
        
        private static double CalculateLackOfCohesionValue(IEnumerable<IFieldSymbol> fieldSymbolList, IReadOnlyCollection<MethodDeclarationSyntax> methodNodeList, Dictionary<IFieldSymbol, int> fieldAccessCounterMap)
        {
            var averageAccessCount = AverageFieldAccesses(fieldSymbolList, fieldAccessCounterMap);
            return ShouldCalculateMetric(methodNodeList) ? 0d : CalculateLackOfCohesionValue(methodNodeList, averageAccessCount);
        }

        private static bool ShouldCalculateMetric(IReadOnlyCollection<MethodDeclarationSyntax> methodNodeList) => 
            methodNodeList.Count < 2;

        private static double CalculateLackOfCohesionValue(IReadOnlyCollection<MethodDeclarationSyntax> methodNodeList, double averageAccessCount) => 
            (methodNodeList.Count - averageAccessCount) / (methodNodeList.Count - 1);

        private static double AverageFieldAccesses(IEnumerable<IFieldSymbol> fieldSymbolList, Dictionary<IFieldSymbol, int> fieldAccessCounterMap) => 
            fieldAccessCounterMap.Values.Sum() / (double)fieldSymbolList.Count();

        private static List<IFieldSymbol> GetFieldSymbolList(SemanticModel semanticModel, BaseTypeDeclarationSyntax typeNode) => 
            semanticModel
                .GetDeclaredSymbol(typeNode)
                .GetMembers()
                .OfType<IFieldSymbol>()
                .ToList();
    }
}