using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.LongConstantSimplifier;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class LongConstantSimplifierDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public LongConstantSimplifierDiagnosticAnalyzer()
            : base(new LongConstantSimplifierRefactoring())
        {
        }
    }
}
