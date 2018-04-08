using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.Refactorings.MethodPropertyIdentifierConvention;
using System.Composition;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class MethodPropertyIdentifierConventionCodeFixProvider : CodeSmellCodeFixProvider
    {
        public MethodPropertyIdentifierConventionCodeFixProvider()
            : base(new MethodPropertyIdentifierConventionRefactoring())
        {
        }
    }
}