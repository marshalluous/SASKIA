using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.UseOfVar;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseOfVarDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public UseOfVarDiagnosticAnalyzer()
            : base(new UseOfVarRefactoring())
        {
        }
    }
}
