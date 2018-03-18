using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.IfAndElseBlockEquals;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class IfAndElseBlockEqualsDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public IfAndElseBlockEqualsDiagnosticAnalyzer()
            : base(new IfAndElseBlockEqualsRefactoring())
        {
        }
    }
}
