using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.DepthOfInheritance;

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
