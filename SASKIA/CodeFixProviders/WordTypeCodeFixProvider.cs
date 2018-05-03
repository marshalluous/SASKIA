﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.Refactorings.DictionaryRefactoring;

namespace SASKIA.CodeFixProviders
{
	[ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class WordTypeCodeFixProvider : CodeSmellCodeFixProvider
    {
        public WordTypeCodeFixProvider()
            : base(new WordTypeRefactoring())
        {
        }
    }
}
