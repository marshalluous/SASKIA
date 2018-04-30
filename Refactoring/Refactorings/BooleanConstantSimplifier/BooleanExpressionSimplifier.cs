using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace Refactoring.Refactorings.BooleanConstantSimplifier
{
    internal static class BooleanExpressionSimplifier
    {
        public static bool? SimplifyUnaryExpression(SyntaxKind operatorKind, bool expression)
        {
            return GetUnarySimplifier(operatorKind)
                .Invoke(expression);
        }

        public static bool? SimplifyBinaryExpression(SyntaxKind operatorKind, bool leftHandSide, bool rightHandSide)
        {
            return GetBinarySimplifier(operatorKind)
                .Invoke(leftHandSide, rightHandSide);
        }

        private static Func<bool, bool?> GetUnarySimplifier(SyntaxKind operatorKind)
        {
            var unarySimplifiers = UnarySimplifiers();

            if (unarySimplifiers.ContainsKey(operatorKind))
            {
                return unarySimplifiers[operatorKind];
            }

            return x => null;
        }

        private static Func<bool, bool, bool?> GetBinarySimplifier(SyntaxKind operatorKind)
        {
            var binarySimplifiers = BinarySimplifiers();

            if (binarySimplifiers.ContainsKey(operatorKind))
            {
                return binarySimplifiers[operatorKind];
            }

            return (x, y) => null;
        }

        private static Dictionary<SyntaxKind, Func<bool, bool, bool?>> BinarySimplifiers()
        {
            return new Dictionary<SyntaxKind, Func<bool, bool, bool?>>
            {
                [SyntaxKind.AmpersandAmpersandToken] = (x, y) => x && y,
                [SyntaxKind.BarBarToken] = (x, y) => x || y
            };
        }

        private static Dictionary<SyntaxKind, Func<bool, bool?>> UnarySimplifiers()
        {
            return new Dictionary<SyntaxKind, Func<bool, bool?>>
            {
                [SyntaxKind.ExclamationToken] = x => !x
            };
        }
    }
}