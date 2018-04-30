using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.Refactorings.IllegalFieldAccess;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class IllegalFieldAccessCodeFixProvider : CodeSmellCodeFixProvider
    {
        public IllegalFieldAccessCodeFixProvider()
            : base(new IllegalFieldAccessRefactoring())
        {
        }
    }
}