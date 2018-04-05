using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Refactoring.DictionaryRefactorings;

namespace SASKIA.DiagnosticAnalyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public sealed class ClassNameDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
	{
		public ClassNameDiagnosticAnalyzer() 
			: base(new ClassNameRefactoring())
		{
		}
	}
}
