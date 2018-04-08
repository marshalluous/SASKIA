using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.DictionaryRefactorings;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    class ClassNameTypoCodeFixProvider : CodeSmellCodeFixProvider
    {
        public ClassNameTypoCodeFixProvider()
            : base(new TypoRefactoring())
        {
        }
    }
}
