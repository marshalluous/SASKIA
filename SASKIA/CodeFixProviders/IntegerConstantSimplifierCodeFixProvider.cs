using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.Refactorings.IntegerConstantSimplifier;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class IntegerConstantSimplifierCodeFixProvider : CodeSmellCodeFixProvider
    {
        public IntegerConstantSimplifierCodeFixProvider()
            : base(new IntegerConstantSimplifierRefactoring())
        {
        }
    }
}