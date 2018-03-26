using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.Refactorings.NotOperatorInversion;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class NotOperationInversionCodeFixProvider : CodeSmellCodeFixProvider
    {
        public NotOperationInversionCodeFixProvider()
            : base(new NotOperatorInversionRefactoring())
        {
        }
    }
}