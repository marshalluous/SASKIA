using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring;
using SASKIA;

namespace RoslynVsixSandbox
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class IfReturnDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public IfReturnDiagnosticAnalyzer()
            : base(new IfReturnRefactoring())
        {
        }
    }
}