using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.Refactorings.DeMorganSimplifier;
using System.Composition;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class DeMorganSimplifierCodeFixProvider : CodeSmellCodeFixProvider
    {
        public DeMorganSimplifierCodeFixProvider()
            : base(new DeMorganSimplifierRefactoring())
        {
        }
    }
}
