using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.Refactorings.TypeIdentifierConvention;
using System.Composition;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class TypeIdentifierConventionCodeFixProvider : CodeSmellCodeFixProvider
    {
        public TypeIdentifierConventionCodeFixProvider()
            : base(new TypeIdentifierConventionRefactoring())
        {
        }
    }
}
