using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.LongParameterList;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class LongParameterListDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public LongParameterListDiagnosticAnalyzer()
            : base(new LongParameterListRefactoring())
        {
        }
    }
}
