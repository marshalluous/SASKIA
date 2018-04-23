using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.LackOfCohesion
{
    internal sealed class LackOfCohesionCounterVisitor : CSharpSyntaxVisitor<IEnumerable<IFieldSymbol>>
    {
        private readonly SemanticModel semanticModel;

        public LackOfCohesionCounterVisitor(SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
        }

        public override IEnumerable<IFieldSymbol> DefaultVisit(SyntaxNode node)
        {
            return node.ChildNodes().SelectMany(Visit);
        }

        public override IEnumerable<IFieldSymbol> VisitIdentifierName(IdentifierNameSyntax node)
        {
            var symbolInfo = semanticModel.GetSymbolInfo(node);

            if (symbolInfo.Symbol is IFieldSymbol fieldSymbol)
            {
                yield return fieldSymbol;
            }
        }
    }
}