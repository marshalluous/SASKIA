﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.Refactorings.DictionaryRefactoring;

namespace SASKIA.CodeFixProviders
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    class TypoCodeFixProvider : CodeSmellCodeFixProvider
    {
        public TypoCodeFixProvider()
            : base(new TypoRefactoring())
        {
        }
    }
}
