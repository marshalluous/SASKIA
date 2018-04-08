using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Refactoring.DictionaryRefactorings;

namespace SASKIA.DiagnosticAnalyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public sealed class TypoDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
	{
		public TypoDiagnosticAnalyzer() 
			: base(new TypoRefactoring())
		{
		}
	}
}
