using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.Refactorings.IfReturnBoolean;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class IfReturnBooleanCodeFixProvider : CodeSmellCodeFixProvider
    {
        public IfReturnBooleanCodeFixProvider()
            : base(new IfReturnBooleanRefactoring())
        {
        }
    }
}
