using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.LackOfCohesion;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class LackOfCohesionDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public LackOfCohesionDiagnosticAnalyzer()
            : base(new LackOfCohesionRefactoring())
        {
        }
    }
}