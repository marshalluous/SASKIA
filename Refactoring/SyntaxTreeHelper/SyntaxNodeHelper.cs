using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Simplification;

namespace Refactoring.SyntaxTreeHelper
{
    internal static class SyntaxNodeHelper
    {
        public static bool CheckNodeText(SyntaxNode syntaxNode, string expectedText) => 
            expectedText == GetText(syntaxNode);

        public static bool CheckNodeType(SyntaxNode syntaxNode, Type nodeType) => 
            syntaxNode.GetType() == nodeType;

        public static bool CheckNodeTypeAndText(SyntaxNode syntaxNode, Type nodeType, string expectedText) => 
            CheckNodeType(syntaxNode, nodeType) && CheckNodeText(syntaxNode, expectedText);

        public static string GetText(SyntaxNode syntaxNode) => 
            syntaxNode.GetText().ToString().Trim();

        public static T FindAncestorOfType<T>(SyntaxToken syntaxToken)
            where T : SyntaxNode => 
            (T) FindAncestorWithPredicate(syntaxToken, node => node is T);

        public static T FindAncestorOfTypeWithText<T>(SyntaxToken syntaxToken, string text)
            where T : SyntaxNode => 
            (T) FindAncestorWithPredicate(syntaxToken, node => node is T && GetText(node) == text);

        public static SyntaxNode FindAncestorWithPredicate(SyntaxNode node, Predicate<SyntaxNode> predicate)
        {
            while (!predicate(node))
                node = node.Parent;
            return node;
        }

        public static SyntaxNode FindAncestorWithPredicate(SyntaxToken syntaxToken, Predicate<SyntaxNode> predicate) =>
            FindAncestorWithPredicate(syntaxToken.Parent, predicate);

        public static ExpressionSyntax AddParentheses(ExpressionSyntax expression)
        {
            if (expression is BinaryExpressionSyntax)
                return SyntaxFactory
                    .ParenthesizedExpression(expression.NormalizeWhitespace())
                    .WithAdditionalAnnotations(Simplifier.Annotation);

            return expression;
        }

        public static IEnumerable<SyntaxKind> GetExpressionSyntaxKinds() => 
            typeof(SyntaxKind)
                .GetEnumNames()
                .Where(name => name.EndsWith("Expression"))
                .Select(name => (SyntaxKind) Enum.Parse(typeof(SyntaxKind), name));
    }
}