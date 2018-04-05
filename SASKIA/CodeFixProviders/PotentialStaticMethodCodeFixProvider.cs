using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.Refactorings.PotentialStaticMethod;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class PotentialStaticMethodCodeFixProvider : CodeSmellCodeFixProvider
    {
        public PotentialStaticMethodCodeFixProvider()
            : base(new PotentialStaticMethodRefactoring())
        {
        }
    }
}