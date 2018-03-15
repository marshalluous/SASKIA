using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ConditionalComplexityDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public ConditionalComplexityDiagnosticAnalyzer()
            : base(new ConditionalComplexityRefactoring())
        {
        }
    }
}
