using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Refactorings.IfReturnBoolean
{
    public sealed class IfReturnBooleanRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA002";

        public string Title => "Unnecessary If Statement";

        public string Description => Title;

        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax) node;
            var thenBlockNode = ifNode.Statement;

            if (ifNode.Else == null)
                return new[] {node};

            var elseBlockNode = ifNode.Else.Statement;

            if (!(thenBlockNode is BlockSyntax thenBlock) || !(elseBlockNode is BlockSyntax elseBlock))
                return new[] {node};

            if (!CompareTrees(thenBlock.Statements, elseBlock.Statements)) return thenBlock.Statements;

            return new[] {node};
        }

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax) node;
            var thenBlockNode = ifNode.Statement;

            if (ifNode.Else == null)
                return DiagnosticInfo.CreateSuccessfulResult();

            var elseBlockNode = ifNode.Else.Statement;

            if (!(thenBlockNode is BlockSyntax thenBlock) ||
                !(elseBlockNode is BlockSyntax elseBlock))
                return DiagnosticInfo.CreateSuccessfulResult();

            return !CompareTrees(thenBlock.Statements, elseBlock.Statements)
                ? DiagnosticInfo.CreateFailedResult("Unnecessary If statement detected")
                : DiagnosticInfo.CreateSuccessfulResult();
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            var result = token.Parent;

            while (result != null && !(result is IfStatementSyntax)) result = result.Parent;

            return result;
        }

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize()
        {
            return new[] {SyntaxKind.IfStatement};
        }


        private bool IsReturnBoolLiteralNode(SyntaxNode node, string booleanLiteral)
        {
            if (node is ReturnStatementSyntax returnNode)
                if (returnNode.Expression is LiteralExpressionSyntax literalNode)
                    return literalNode.GetText().ToString().Trim() == booleanLiteral;

            return false;
        }

        private bool CompareSyntaxNodeLists(IReadOnlyList<SyntaxNode> nodeList1, IReadOnlyList<SyntaxNode> nodeList2)
        {
            if (nodeList1.Count != nodeList2.Count)
                return false;

            var equals = true;

            for (var index = 0; index < nodeList1.Count; ++index)
                equals = equals && CompareSyntaxNode(nodeList1[index], nodeList2[index]);

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

            var treeNotEquals = true;

            int index;

            for (index = 0; index < thenStatements.Count - 1; ++index)
                treeNotEquals = treeNotEquals && !CompareSyntaxNode(thenStatements[index], elseStatements[index]);

            if (IsReturnBoolLiteralNode(thenStatements[index], "true") &&
                IsReturnBoolLiteralNode(elseStatements[index], "false"))
                return false;
            if (IsReturnBoolLiteralNode(thenStatements[index], "false") &&
                IsReturnBoolLiteralNode(elseStatements[index], "true"))
                return false;
            treeNotEquals = treeNotEquals && !CompareSyntaxNode(thenStatements[index], elseStatements[index]);

            return treeNotEquals;
        }
    }
}