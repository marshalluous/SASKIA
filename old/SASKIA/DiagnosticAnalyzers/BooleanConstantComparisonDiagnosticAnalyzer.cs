using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Refactoring.Refactorings.BooleanConstantComparison;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BooleanConstantComparisonDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public BooleanConstantComparisonDiagnosticAnalyzer()
            : base(new BooleanConstantComparisonRefactoring())
        {
        }
    }
}