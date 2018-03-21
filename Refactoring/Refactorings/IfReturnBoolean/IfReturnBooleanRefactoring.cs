using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.IfReturnBoolean
{
    public sealed class IfReturnBooleanRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA111";

        public string Title => DiagnosticId;

        public string Description => Title;

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.IfStatement};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax) node;
            var thenNode = ifNode.Statement;

            if (ifNode.Else == null)
                return DiagnosticInfo.CreateSuccessfulResult();

            var elseNode = ifNode.Else.Statement;

            if (thenNode is BlockSyntax thenBlock && thenBlock.ChildNodes().Count() == 1)
            {
                thenNode = thenBlock.Statements.First();
            }

            if (elseNode is BlockSyntax elseBlock && elseBlock.ChildNodes().Count() == 1)
            {
                elseNode = elseBlock.Statements.First();
            }

            if ((IsReturnBooleanStatement(thenNode, "true") && IsReturnBooleanStatement(elseNode, "false")) ||
                IsReturnBooleanStatement(thenNode, "false") && IsReturnBooleanStatement(elseNode, "true"))
            {
                return DiagnosticInfo.CreateFailedResult("Unnecessary If-statement");
            }

            return DiagnosticInfo.CreateSuccessfulResult();
        }

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax) node;
            var thenNode = ifNode.Statement;

            if (ifNode.Else == null)
                return new [] { node };
            
            var elseNode = ifNode.Else.Statement;
            
            if (thenNode is BlockSyntax thenBlock && thenBlock.ChildNodes().Count() == 1)
            {
                thenNode = thenBlock.Statements.First();
            }

            if (elseNode is BlockSyntax elseBlock && elseBlock.ChildNodes().Count() == 1)
            {
                elseNode = elseBlock.Statements.First();
            }

            if (IsReturnBooleanStatement(thenNode, "true") && IsReturnBooleanStatement(elseNode, "false"))
            {
                return new[] {SyntaxFactory.ReturnStatement(ifNode.Condition)};
            }

            if (!IsReturnBooleanStatement(thenNode, "false") || !IsReturnBooleanStatement(elseNode, "true"))
                return new[] {node};

            var notNode = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, ifNode.Condition);
            return new[] { SyntaxFactory.ReturnStatement(notNode)};
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return SyntaxNodeHelper.FindAncestorOfType<IfStatementSyntax>(token);
        }

        private static bool IsReturnBooleanStatement(SyntaxNode statementNode, string booleanLiteral)
        {
            return statementNode is ReturnStatementSyntax returnNode &&
                   SyntaxNodeHelper.GetText(returnNode.Expression) == booleanLiteral;
        }
    }
}
