using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.Refactorings.UseOfVar;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class UseOfVarCodeFixProvider : CodeSmellCodeFixProvider
    {
        public UseOfVarCodeFixProvider()
            : base(new UseOfVarRefactoring())
        {
        }
    }
}
