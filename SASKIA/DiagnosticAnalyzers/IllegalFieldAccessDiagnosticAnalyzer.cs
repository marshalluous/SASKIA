using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.IllegalFieldAccess;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class IllegalFieldAccessDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public IllegalFieldAccessDiagnosticAnalyzer() 
            : base(new IllegalFieldAccessRefactoring())
        {
        }
    }
}