﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring;

namespace SASKIA
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ConstExpressionDiagnosticAnalyzer : CodeSmellDiagnosticAnalyzer
    {
        public ConstExpressionDiagnosticAnalyzer()
            : base(new ConstExpressionRefactoring())
        {
        }
    }
}