using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.TypeIdentifierConvention;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class TypeIdentifierConventionDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public TypeIdentifierConventionDiagnosticAnalyzer()
            : base(new TypeIdentifierConventionRefactoring())
        {
        }
    }
}
