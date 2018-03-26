using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.LinesOfCode;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class LinesOfCodeDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public LinesOfCodeDiagnosticAnalyzer()
            : base(new LinesOfCodeRefactoring())
        {
        }
    }
}
