﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.IntegerConstantSimplifier
{
    public sealed class IntegerConstantSimplifierRefactoring : IRefactoring
    {
        public string DiagnosticId => RefactoringId.IntegerConstantSimplifier.GetDiagnosticId();

        public string Title => RefactoringMessageFactory.IntegerConstantSimplifierTitle();

        public string Description => RefactoringMessageFactory.IntegerConstantSimplifierDescription();

        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            SyntaxNodeHelper.GetExpressionSyntaxKinds();

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            var parentValue = EvaluateValue(node.Parent);

            if (node is LiteralExpressionSyntax || parentValue != null)
                return DiagnosticInfo.CreateSuccessfulResult();
            
            var value = EvaluateValue(node);

            return value == null ? 
                DiagnosticInfo.CreateSuccessfulResult() :
                DiagnosticInfo.CreateFailedResult(RefactoringMessageFactory.IntegerConstantSimplifierMessage(value.Value));
        }
        
        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            var value = EvaluateValue(node);

            if (value == null || node is LiteralExpressionSyntax)
                return null;
            
            var literal = SyntaxFactory.Literal(value.Value);
            return new [] { SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, literal) };
        }

        private static int? EvaluateValue(SyntaxNode node)
        {
            var visitor = new IntegerConstantSimplifierVisitor();
            return visitor.Visit(node);
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            var node = token.Parent;

            while (EvaluateValue(node.Parent) != null)
                node = node.Parent;

            return node;
        }
    }
}
