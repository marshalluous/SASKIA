using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Refactoring.DictionaryRefactorings;

namespace SASKIA.DiagnosticAnalyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public sealed class ClassNameTypoDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
	{
		public ClassNameTypoDiagnosticAnalyzer() 
			: base(new TypoRefactoring())
		{
		}
	}
}
