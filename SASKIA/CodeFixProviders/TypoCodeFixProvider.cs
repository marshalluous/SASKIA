using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.Refactorings.DictionaryRefactoring;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class TypoCodeFixProvider : CodeSmellCodeFixProvider
    {
        public TypoCodeFixProvider()
            : base(new TypoRefactoring())
        {
        }
    }
}
