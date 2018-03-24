using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.NotOperatorInversion;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NotOperatorInversionDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public NotOperatorInversionDiagnosticAnalyzer()
            : base(new NotOperatorInversionRefactoring())
        {
        }
    }
}
