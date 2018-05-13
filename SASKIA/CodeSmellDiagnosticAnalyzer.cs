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
                true);
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

                var diagnostic = CreateDiagnostic(ref context, diagnosticInfo);
                context.ReportDiagnostic(diagnostic);
                
                var location = diagnostic.Location;
                var fileName = location.SourceTree.FilePath;
                var line = location.GetLineSpan().StartLinePosition.Line;
                var column = location.GetLineSpan().StartLinePosition.Character;
                    
                File.AppendAllText(@"C:\temp\evaluation.csv",
                    GetType().Name + ";" + context.Compilation.Assembly + ";" + fileName + ";" + line + ";" + column + ";" +
                    diagnosticInfo.AdditionalInformation + ";" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\r\n");
            }
            catch (Exception exception)
            {
                File.AppendAllText(@"C:\temp\log.txt", this.GetType().Name + ": " + exception.Message + "\r\n");
            }
        }

        private Diagnostic CreateDiagnostic(ref SyntaxNodeAnalysisContext context, DiagnosticInfo diagnosticInfo)
        {
            var markableLocation = diagnosticInfo.MarkableLocation ?? context.Node.GetLocation();
            return Diagnostic.Create(CreateRule(diagnosticInfo.Message), markableLocation);
        }
    }
}