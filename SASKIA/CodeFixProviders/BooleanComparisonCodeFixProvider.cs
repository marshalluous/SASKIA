using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class BooleanComparisonCodeFixProvider : CodeSmellCodeFixProvider
    {
        public BooleanComparisonCodeFixProvider()
            : base(new BooleanComparisonRefactoring())
        {
        }
    }
}