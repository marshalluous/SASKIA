using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.DictionaryRefactorings;

namespace SASKIA.CodeFixProviders
{
	[ExportCodeFixProvider(LanguageNames.CSharp), Shared]
	public sealed class ClassNameCodeFixProvider : CodeSmellCodeFixProvider
	{
		public ClassNameCodeFixProvider() 
			: base(new ClassNameRefactoring())
		{
		}
	}
}
