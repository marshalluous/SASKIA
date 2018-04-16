using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.DeMorganSimplifier;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DeMorganSimplifierDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public DeMorganSimplifierDiagnosticAnalyzer()
            : base(new DeMorganSimplifierRefactoring())
        {
        }
    }
}
