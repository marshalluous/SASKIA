using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.IntegerConstantSimplifier;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class IntegerConstantSimplifierDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public IntegerConstantSimplifierDiagnosticAnalyzer()
            : base(new IntegerConstantSimplifierRefactoring())
        {
        }
    }
}
