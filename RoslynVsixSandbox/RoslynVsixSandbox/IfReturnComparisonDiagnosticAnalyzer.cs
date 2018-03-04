using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RoslynVsixSandbox
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class IfReturnDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "CS_002";
        private const string Title = "Nasty If";
        private const string Message = "'Nasty if pattern detected";
        private const string Categoty = "CodeSmell";
        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(CreateRule(string.Empty));
        
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(IfDetected, SyntaxKind.IfStatement);
        }

        private static DiagnosticDescriptor CreateRule(string pattern)
        {
            return new DiagnosticDescriptor(DiagnosticId, Title, Message, Categoty,
                DiagnosticSeverity.Error, isEnabledByDefault: true, description: $"Just write return {pattern}");
        }
        
        private bool IsReturnBoolLiteralNode(SyntaxNode node, string booleanLiteral)
        {
            if (node is ReturnStatementSyntax returnNode)
            {
                if (returnNode.Expression is LiteralExpressionSyntax literalNode)
                {
                    return literalNode.GetText().ToString().Trim() == booleanLiteral;
                }
            }

            return false;
        }

        private bool CompareSyntaxNodeLists(List<SyntaxNode> nodeList1, List<SyntaxNode> nodeList2)
        {
            if (nodeList1.Count() != nodeList2.Count())
                return false;

            var equals = true;
            
            for (var index = 0; index < nodeList1.Count(); ++index)
            {
                equals = equals && CompareSyntaxNode(nodeList1[index], nodeList2[index]);
            }

            return equals;
        }

        private bool CompareSyntaxNode(SyntaxNode node1, SyntaxNode node2)
        {
            return node1.GetText().ToString().Trim() == node2.GetText().ToString().Trim() &&
                CompareSyntaxNodeLists(node1.ChildNodes().ToList(), node2.ChildNodes().ToList());
        }
        
        private bool CompareTrees(SyntaxList<SyntaxNode> thenStatements, SyntaxList<SyntaxNode> elseStatements)
        {
            if (thenStatements.Count != elseStatements.Count)
                return false;

            bool r = true;

            int index;

            for (index = 0; index < thenStatements.Count - 1; ++index)
            {
                r = r && !CompareSyntaxNode(thenStatements[index], elseStatements[index]);
            }

            if (IsReturnBoolLiteralNode(thenStatements[index], "true") &&
                IsReturnBoolLiteralNode(elseStatements[index], "false"))
            {
                return false;
            }
            else if (IsReturnBoolLiteralNode(thenStatements[index], "false") &&
                IsReturnBoolLiteralNode(elseStatements[index], "true"))
            {
                return false;
            }
            else
            {
                r = r && !CompareSyntaxNode(thenStatements[index], elseStatements[index]);
            }

            return r;
        }

        private void CreateDiagnostic(DiagnosticDescriptor rule, SyntaxNodeAnalysisContext context)
        {
            var diagnostic = Diagnostic.Create(rule, context.Node.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }

        private void IfDetected(SyntaxNodeAnalysisContext context)
        {
            var ifNode = (IfStatementSyntax) context.Node;
            var thenBlockNode = ifNode.Statement;

            if (ifNode.Else == null)
                return;

            var elseBlockNode = ifNode.Else.Statement;
            var conditionText = ifNode.Condition.GetText().ToString();
            
            if (thenBlockNode is BlockSyntax thenBlock && elseBlockNode is BlockSyntax elseBlock)
            {
                if (!CompareTrees(thenBlock.Statements, elseBlock.Statements))
                {
                    CreateDiagnostic(CreateRule("same nodes"), context);
                }
            }
        }
    }
}