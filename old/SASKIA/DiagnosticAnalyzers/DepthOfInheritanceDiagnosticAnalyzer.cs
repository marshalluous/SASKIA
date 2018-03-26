using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.DepthOfInheritance;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DepthOfInheritanceDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public DepthOfInheritanceDiagnosticAnalyzer() 
            : base(new DepthOfInheritanceRefactoring())
        {
        }
    }
}
