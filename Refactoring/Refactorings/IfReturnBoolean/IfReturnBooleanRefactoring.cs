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
        public string DiagnosticId => RefactoringId.IfReturnBoolean.GetDiagnosticId();

        public string Title => RefactoringMessageFactory.IfReturnBooleanTitle();

        public string Description => RefactoringMessageFactory.IfReturnBooleanDescription();


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.IfStatement};

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var fixableNodes = GetFixableNodes(node);
            var ifNode = (IfStatementSyntax)node;

            return fixableNodes == null ? 
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult(
                    RefactoringMessageFactory.IfReturnBooleanMessage(fixableNodes.First().ToString()), 
                    markableLocation: ifNode.IfKeyword.GetLocation());
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax)node;

            var thenNode = GetThenBlock(ifNode);
            var elseNode = GetElseBlock(ifNode);

            if (elseNode == null)
                return null;

            if (IsReturnBooleanStatement(thenNode, "true") && IsReturnBooleanStatement(elseNode, "false"))
            {
                return new[] { SyntaxFactory.ReturnStatement(ifNode.Condition)
                    .NormalizeWhitespace() };
            }

            if (!IsReturnBooleanStatement(thenNode, "false") || !IsReturnBooleanStatement(elseNode, "true"))
                return null;

            var condition = SyntaxNodeHelper.AddParentheses(ifNode.Condition);
            var notNode = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, condition);

            return new[] { SyntaxFactory.ReturnStatement(notNode).NormalizeWhitespace() };
        }

        private static StatementSyntax GetElseBlock(IfStatementSyntax ifNode)
        {
            if (ifNode.Else == null)
                return null;

            var elseNode = ifNode.Else.Statement;

            if (elseNode is BlockSyntax elseBlock && elseBlock.ChildNodes().Count() == 1)
            {
                return elseBlock.Statements.First();
            }

            return elseNode;
        }

        private static StatementSyntax GetThenBlock(IfStatementSyntax ifNode)
        {
            var thenNode = ifNode.Statement;

            if (thenNode is BlockSyntax thenBlock && thenBlock.ChildNodes().Count() == 1)
            {
                return thenBlock.Statements.First();
            }

            return thenNode;
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
