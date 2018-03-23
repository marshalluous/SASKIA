using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace Refactoring.Refactorings.IntegerConstantSimplifier
{
    internal static class IntegerConstantOperatorSimplifier
    {
        public static int? ReducePrefixUnaryOperation(SyntaxKind operatorKind, int operand)
        {
            var operatorFunc = new Dictionary<SyntaxKind, Func<int, int?>>
            {
                [SyntaxKind.PlusToken] = x => x,
                [SyntaxKind.MinusToken] = x => -x,
                [SyntaxKind.TildeToken] = x => ~x
            };
            
            return operatorFunc.ContainsKey(operatorKind) ?
                operatorFunc[operatorKind].Invoke(operand) :
                null;
        }

        public static int? ReduceBinaryOperation(SyntaxKind operatorKind, int left, int right)
        {
            var operatorFunc = new Dictionary<SyntaxKind, Func<int, int, int?>>
            {
                [SyntaxKind.PlusToken] = (x, y) => x + y,
                [SyntaxKind.MinusToken] = (x, y) => x - y,
                [SyntaxKind.AsteriskToken] = (x, y) => x * y,
                [SyntaxKind.PercentToken] = (x, y) => y == 0 ? null : ((int?)x % y),
                [SyntaxKind.SlashToken] = (x, y) => y == 0 ? null : (int?)(x / y),
                [SyntaxKind.LessThanLessThanToken] = (x, y) => x << y,
                [SyntaxKind.GreaterThanGreaterThanToken] = (x, y) => x >> y,
                [SyntaxKind.BarToken] = (x, y) => x | y,
                [SyntaxKind.AmpersandToken] = (x, y) => x & y,
                [SyntaxKind.CaretToken] = (x, y) => x ^ y
            };
            
            return operatorFunc.ContainsKey(operatorKind) ?
                operatorFunc[operatorKind].Invoke(left, right) :
                null;
        }
    }
}
