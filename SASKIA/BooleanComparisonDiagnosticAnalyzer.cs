using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring;

namespace SASKIA
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BooleanComparisonDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public BooleanComparisonDiagnosticAnalyzer()
            : base(new BooleanComparisonRefactoring())
        {
        }
    }
}