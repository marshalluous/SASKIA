using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Refactoring.BooleanConstantSimplifier;

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