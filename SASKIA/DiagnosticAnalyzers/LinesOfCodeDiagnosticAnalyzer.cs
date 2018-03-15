using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring;

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
