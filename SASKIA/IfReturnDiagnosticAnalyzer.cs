using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring;

namespace SASKIA
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