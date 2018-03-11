using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring;
using System.Composition;

namespace SASKIA
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