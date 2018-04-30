using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.Refactorings.LongConstantSimplifier;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class LongConstantSimplifierCodeFixProvider : CodeSmellCodeFixProvider
    {
        public LongConstantSimplifierCodeFixProvider()
            : base(new LongConstantSimplifierRefactoring())
        {
        }
    }
}
