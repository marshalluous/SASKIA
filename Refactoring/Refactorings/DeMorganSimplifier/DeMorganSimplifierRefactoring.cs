using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.DeMorganSimplifier
{
    public sealed class DeMorganSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.DeMorganSimplifier.GetDiagnosticId();

        public string Title => DiagnosticId;

        public string Description => Title;

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            return GetFixableNodes(node) == null ?
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult("DeMorgan");
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var notNode = (PrefixUnaryExpressionSyntax)node;

            if (notNode.ChildNodes().First() is ParenthesizedExpressionSyntax parenthesizedNode)
            {
                var expressionNode = parenthesizedNode.Expression;
                
                if (expressionNode is BinaryExpressionSyntax binaryExpressionNode)
                {
                    var leftNode = binaryExpressionNode.Left;
                    var rightNode = binaryExpressionNode.Right;
                    var notLeftNode = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxNodeHelper.AddParentheses(leftNode));
                    var notRightNode = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, SyntaxNodeHelper.AddParentheses(rightNode));
                    
                    if (binaryExpressionNode.OperatorToken.Text == "||")
                    {
                        return CreateResultNode(SyntaxKind.LogicalAndExpression, notLeftNode, notRightNode);
                    }

                    if (binaryExpressionNode.OperatorToken.Text == "&&")
                    {
                        return CreateResultNode(SyntaxKind.LogicalOrExpression, notLeftNode, notRightNode);
                    }
                }
            }

            return null;
        }

        private static IEnumerable<SyntaxNode> CreateResultNode(SyntaxKind operatorKind, ExpressionSyntax leftHandSide,
            ExpressionSyntax rightHandSide)
        {
            return new[] { SyntaxFactory.BinaryExpression(operatorKind, leftHandSide, rightHandSide)
                .NormalizeWhitespace() };
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            return SyntaxNodeHelper.FindAncestorOfType<PrefixUnaryExpressionSyntax>(token);
        }

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize()
        {
            return new[] { SyntaxKind.LogicalNotExpression };
        }
    }
}
