﻿using System;
using Microsoft.CodeAnalysis;

namespace Refactoring.Helper
{
    internal static class SyntaxNodeHelper
    {
        public static bool CheckNodeText(SyntaxNode syntaxNode, string expectedText)
        {
            return expectedText == GetText(syntaxNode);
        }

        public static bool CheckNodeType(SyntaxNode syntaxNode, Type nodeType)
        {
            return syntaxNode.GetType() == nodeType;
        }

        public static bool CheckNodeTypeAndText(SyntaxNode syntaxNode, Type nodeType, string expectedText)
        {
            return CheckNodeType(syntaxNode, nodeType) &&
                   CheckNodeText(syntaxNode, expectedText);
        }

        public static string GetText(SyntaxNode syntaxNode)
        {
            return syntaxNode.GetText().ToString().Trim();
        }

        public static T FindAncestorOfType<T>(SyntaxToken syntaxToken)
            where T : SyntaxNode
        {
            return (T) FindAncestorWithPredicate(syntaxToken, node => node is T);
        }

        public static T FindAncestorOfTypeWithText<T>(SyntaxToken syntaxToken, string text)
            where T : SyntaxNode
        {
            return (T) FindAncestorWithPredicate(syntaxToken, node => node is T && GetText(node) == text);
        }

        public static SyntaxNode FindAncestorWithPredicate(SyntaxToken syntaxToken, Predicate<SyntaxNode> predicate)
        {
            var node = syntaxToken.Parent;

            while (!predicate(node)) node = node.Parent;

            return node;
        }
    }
}