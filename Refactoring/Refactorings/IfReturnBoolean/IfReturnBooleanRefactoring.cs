using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.IfReturnBoolean
{
    public sealed class IfReturnBooleanRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.IfReturnBoolean.GetDiagnosticId();
        public string Title => RefactoringMessages.IfReturnBooleanTitle();
        public string Description => RefactoringMessages.IfReturnBooleanDescription();
        
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
                CreateFailedDiagnosticResult(fixableNodes, ifNode);
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var ifNode = (IfStatementSyntax)node;
            var elseNode = ifNode.Else?.Statement;

            if (elseNode == null)
                return null;
            
            var thenNode = NormalizeBlock(ifNode.Statement);
            elseNode = NormalizeBlock(elseNode);

            if (IsReturnBooleanStatement(thenNode, "true") && IsReturnBooleanStatement(elseNode, "false"))
                return CreateReturnNode(ifNode.Condition);

            if (IsReturnBooleanStatement(thenNode, "false") && IsReturnBooleanStatement(elseNode, "true"))
                return CreateReturnNode(Not(ifNode.Condition));

            return null;
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token) => 
            SyntaxNodeHelper.FindAncestorOfType<IfStatementSyntax>(token);

        private static IEnumerable<SyntaxNode> CreateReturnNode(ExpressionSyntax expressionNode) => 
            new[] {SyntaxFactory.ReturnStatement(expressionNode).NormalizeWhitespace()};

        private static PrefixUnaryExpressionSyntax Not(ExpressionSyntax condition)
        {
            condition = SyntaxNodeHelper.AddParentheses(condition);
            return SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, condition);
        }

        private static StatementSyntax NormalizeBlock(StatementSyntax ifBlockNode)
        {
            while (ifBlockNode.ChildNodes().Count() == 1 && ifBlockNode is BlockSyntax)
            {
                ifBlockNode = (StatementSyntax) ifBlockNode.ChildNodes().First();
            }

            return ifBlockNode;
        }
        
        private static bool IsReturnBooleanStatement(SyntaxNode statementNode, string booleanLiteral)
        {
            return statementNode is ReturnStatementSyntax returnNode &&
                   SyntaxNodeHelper.GetText(NormalizeReturnValue(returnNode.Expression)) == booleanLiteral;
        }

        private static SyntaxNode NormalizeReturnValue(SyntaxNode node)
        {
            while (node is ParenthesizedExpressionSyntax)
            {
                node = ((ParenthesizedExpressionSyntax) node).Expression;
            }

            return node;
        }

        private static DiagnosticInfo CreateFailedDiagnosticResult(IEnumerable<SyntaxNode> fixableNodes, IfStatementSyntax ifNode) =>
            DiagnosticInfo.CreateFailedResult(RefactoringMessages.IfReturnBooleanMessage(fixableNodes.First().ToString()),
                markableLocation: ifNode.IfKeyword.GetLocation());
    }
}
