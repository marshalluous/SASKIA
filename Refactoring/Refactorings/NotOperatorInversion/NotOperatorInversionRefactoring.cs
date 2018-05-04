using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.SyntaxTreeHelper;

namespace Refactoring.Refactorings.NotOperatorInversion
{
    public sealed class NotOperatorInversionRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.NotOperatorInversion.GetDiagnosticId();
        public string Title => RefactoringMessages.NotOperatorInversionTitle();
        public string Description => RefactoringMessages.NotOperatorInversionDescription();
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);
        
		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.LogicalNotExpression};
            
        public DiagnosticInfo DoDiagnosis(SyntaxNode node) => 
            GetFixableNodes(node) == null ? 
                DiagnosticInfo.CreateSuccessfulResult() : 
                DiagnosticInfo.CreateFailedResult(RefactoringMessages.NotOperatorInversionMessage());

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var logicalNotExpression = (PrefixUnaryExpressionSyntax)node;

            if (IsNotExpression(logicalNotExpression) ||
                EncapsulatesParenthesizedExpression(logicalNotExpression, out var operandExpression))
                return null;
            
            SyntaxNode replaceableNode = null;

            switch (operandExpression)
            {
                case BinaryExpressionSyntax binaryExpression:
                    replaceableNode = ExpressionNotInverter.InvertBinaryExpression(binaryExpression);
                    break;
                case PrefixUnaryExpressionSyntax unaryExpression when unaryExpression.OperatorToken.Kind() == SyntaxKind.ExclamationToken && unaryExpression.Operand is ParenthesizedExpressionSyntax nestedExpression:
                    replaceableNode = nestedExpression.Expression.NormalizeWhitespace();
                    break;
            }

            return replaceableNode == null ? null : new[] { replaceableNode };
        }

        private static bool EncapsulatesParenthesizedExpression(PrefixUnaryExpressionSyntax logicalNotExpression, 
            out ExpressionSyntax operandExpression)
        {
            if (logicalNotExpression.Operand is ParenthesizedExpressionSyntax parenthesizedExpression)
            {
                operandExpression = parenthesizedExpression.Expression;
                return false;
            }

            operandExpression = null;
            return true;
        }

        private static bool IsNotExpression(PrefixUnaryExpressionSyntax logicalNotExpression) => 
            logicalNotExpression.OperatorToken.Kind() != SyntaxKind.ExclamationToken;

        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<PrefixUnaryExpressionSyntax>(token);
    }
}
