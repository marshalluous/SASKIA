using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.Refactorings.BooleanConstantComparison;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class BooleanConstantComparisonCodeFixProvider : CodeSmellCodeFixProvider
    {
        public BooleanConstantComparisonCodeFixProvider() 
            : base(new BooleanConstantComparisonRefactoring())
        {
        }
    }
}