using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.BooleanConstantSimplifier;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class BooleanConstantSimplifierCodeFixProvider : CodeSmellCodeFixProvider
    {
        public BooleanConstantSimplifierCodeFixProvider() 
            : base(new BooleanConstantSimplifierRefactoring())
        {
        }
    }
}