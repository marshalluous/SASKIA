using System;
using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Refactoring;

namespace SASKIA
{
    public abstract class CodeSmellDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string Category = "CodeSmell";
        private readonly IRefactoring refactoring;

        protected CodeSmellDiagnosticAnalyzer(IRefactoring refactoring)
        {
            this.refactoring = refactoring;
        }
       
        private DiagnosticDescriptor CreateRule(string message)
        {
            return new DiagnosticDescriptor(
                refactoring.DiagnosticId, 
                refactoring.Title,
                message,
                Category,
                DiagnosticSeverity.Warning,
                true,
                refactoring.Description);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
            ImmutableArray.Create(CreateRule(string.Empty));

        public override void Initialize(AnalysisContext context)
        {
            foreach (var syntaxKind in refactoring.GetSyntaxKindsToRecognize())
            {
                context.RegisterSyntaxNodeAction(OnDetect, syntaxKind);
            }
        }

        private void OnDetect(SyntaxNodeAnalysisContext context)
        {
            try
            {
                var diagnosticInfo = refactoring.DoDiagnosis(context.Node);

                if (!diagnosticInfo.DiagnosticFound)
                    return;

                var diagnostic = Diagnostic.Create(CreateRule(diagnosticInfo.Message), context.Node.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
            catch (Exception exception)
            {
                File.AppendAllText("log.txt", exception.Message + "\r\n");
            }
        }
    }
}