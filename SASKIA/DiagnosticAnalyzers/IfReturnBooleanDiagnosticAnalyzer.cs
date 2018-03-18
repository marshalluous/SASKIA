using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.IfReturnBoolean;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class IfReturnBooleanDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public IfReturnBooleanDiagnosticAnalyzer()
            : base(new IfReturnBooleanRefactoring())
        {
        }
    }
}