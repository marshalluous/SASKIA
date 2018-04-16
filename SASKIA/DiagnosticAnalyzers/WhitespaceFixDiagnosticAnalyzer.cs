using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring.Refactorings.WhitespaceFix;

namespace SASKIA.DiagnosticAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class WhitespaceFixDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public WhitespaceFixDiagnosticAnalyzer() 
            : base(new WhitespaceFixRefactoring())
        {
        }
    }
}