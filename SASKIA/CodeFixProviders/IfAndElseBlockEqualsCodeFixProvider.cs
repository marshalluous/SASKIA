using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.IfAndElseBlockEquals;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class IfAndElseBlockEqualsCodeFixProvider : CodeSmellCodeFixProvider
    {
        public IfAndElseBlockEqualsCodeFixProvider()
            : base(new IfAndElseBlockEqualsRefactoring())
        {
        }
    }
}
