using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring;
using System.Composition;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class ConstExpressionCodeFixProvider : CodeSmellCodeFixProvider
    {
        public ConstExpressionCodeFixProvider()
            : base(new ConstExpressionRefactoring())
        {
        }
    }
}