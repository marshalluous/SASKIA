using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.PotentialStaticMethod;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class PotentialStaticMethodDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public PotentialStaticMethodDiagnosticAnalyzer()
            : base(new PotentialStaticMethodRefactoring())
        {
        }
    }
}
