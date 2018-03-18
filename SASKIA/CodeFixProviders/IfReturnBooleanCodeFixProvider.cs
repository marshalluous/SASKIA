using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring;
using System.Composition;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class IfReturnCodeFixProvider : CodeSmellCodeFixProvider
    {
        public IfReturnCodeFixProvider()
            : base(new IfReturnRefactoring())
        {
        }
    }
}
