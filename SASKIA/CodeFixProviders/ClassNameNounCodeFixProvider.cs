using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring.DictionaryRefactorings;
using System.Composition;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
	public sealed class ClassNameNounCodeFixProvider : CodeSmellCodeFixProvider
	{
		public ClassNameNounCodeFixProvider() 
			: base(new ClassNameNounRefactoring())
		{
		}
	}
}
