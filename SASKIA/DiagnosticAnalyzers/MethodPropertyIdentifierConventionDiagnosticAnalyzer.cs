using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.MethodPropertyIdentifierConvention;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class MethodPropertyIdentifierConventionDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public MethodPropertyIdentifierConventionDiagnosticAnalyzer()
            : base(new MethodPropertyIdentifierConventionRefactoring())
        {
        }
    }
}
