using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Refactoring.Refactorings.BooleanConstantSimplifier;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BooleanConstantSimplifierDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public BooleanConstantSimplifierDiagnosticAnalyzer()
            : base(new BooleanConstantSimplifierRefactoring())
        {
        }
    }
}