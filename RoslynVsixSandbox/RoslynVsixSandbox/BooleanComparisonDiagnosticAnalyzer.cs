using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RoslynVsixSandbox
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BooleanComparisonDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "CS_001";
        private const string Title = "Nasty boolean comparison";
        private const string Message = "'Nasty boolean comparison' pattern detected";
        private const string Categoty = "CodeSmell";
        private const string Description = "Prevent comparisons with boolean constants";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, Message, Categoty, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(EqualsEqualsNodeDetected, SyntaxKind.EqualsExpression);
            context.RegisterSyntaxNodeAction(EqualsEqualsNodeDetected, SyntaxKind.NotEqualsExpression);
        }

        private bool IsBooleanLiteralNode(ExpressionSyntax expressionNode)
        {
            var literalText = expressionNode.GetText().ToString().Trim();

            return expressionNode is LiteralExpressionSyntax &&
                            (literalText == "true" || literalText == "false");
        }
        
        private void CreateDiagnostic(DiagnosticDescriptor rule, SyntaxNodeAnalysisContext context)
        {
            var diagnostic = Diagnostic.Create(rule, context.Node.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }

        private void EqualsEqualsNodeDetected(SyntaxNodeAnalysisContext context)
        {
            var equalsEqualsNode = (BinaryExpressionSyntax)context.Node;
            
            if (IsBooleanLiteralNode(equalsEqualsNode.Left) || IsBooleanLiteralNode(equalsEqualsNode.Right))
            {
                CreateDiagnostic(Rule, context);
            }
        }
    }
}