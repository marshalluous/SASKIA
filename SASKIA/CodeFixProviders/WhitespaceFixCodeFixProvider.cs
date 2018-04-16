using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.Refactorings.WhitespaceFix;
using System.Composition;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class WhitespaceFixCodeFixProvider : CodeSmellCodeFixProvider
    {
        public WhitespaceFixCodeFixProvider()
            : base(new WhitespaceFixRefactoring())
        {
        }
    }
}
