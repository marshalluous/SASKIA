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

        public string Title => DiagnosticId;

        public string Description => Title;
        
		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);


		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            new[] {SyntaxKind.LogicalNotExpression};
            
        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var resultNode = GetFixableNodes(node);
            return resultNode == null ? 
                DiagnosticInfo.CreateSuccessfulResult() : 
                DiagnosticInfo.CreateFailedResult("failed result");
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var logicalNotExpression = (PrefixUnaryExpressionSyntax) node;

            if (logicalNotExpression.OperatorToken.Kind() != SyntaxKind.ExclamationToken)
                return null;

            if (!(logicalNotExpression.Operand is ParenthesizedExpressionSyntax operandExpression))
                return null;

            var expression = operandExpression.Expression;
            
            switch (expression)
            {
                case BinaryExpressionSyntax binaryExpression:
                    return new [] { ExpressionNotInverter.InvertBinaryExpression(binaryExpression) };
                case PrefixUnaryExpressionSyntax unaryExpression when unaryExpression.OperatorToken.Kind() == SyntaxKind.ExclamationToken && unaryExpression.Operand is ParenthesizedExpressionSyntax nestedExpression:
                    return new[] { nestedExpression.Expression.NormalizeWhitespace() };
            }

            return null;
        }
        
        public SyntaxNode GetReplaceableNode(SyntaxToken token) =>
            SyntaxNodeHelper.FindAncestorOfType<PrefixUnaryExpressionSyntax>(token);
    }
}
