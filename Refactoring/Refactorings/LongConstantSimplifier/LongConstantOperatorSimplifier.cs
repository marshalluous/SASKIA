using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace Refactoring.Refactorings.LongConstantSimplifier
{
    internal static class LongConstantOperatorSimplifier
    {
        public static long? ReducePrefixUnaryOperation(SyntaxKind operatorKind, long operand)
        {
            var operatorFunc = new Dictionary<SyntaxKind, Func<long, long?>>
            {
                [SyntaxKind.PlusToken] = x => x,
                [SyntaxKind.MinusToken] = x => -x,
                [SyntaxKind.TildeToken] = x => ~x
            };
            
            return operatorFunc.ContainsKey(operatorKind) ?
                operatorFunc[operatorKind].Invoke(operand) :
                null;
        }

        public static long? ReduceBinaryOperation(SyntaxKind operatorKind, long left, long right)
        {
            var operatorFunc = new Dictionary<SyntaxKind, Func<long, long, long?>>
            {
                [SyntaxKind.PlusToken] = (x, y) => x + y,
                [SyntaxKind.MinusToken] = (x, y) => x - y,
                [SyntaxKind.AsteriskToken] = (x, y) => x * y,
                [SyntaxKind.PercentToken] = (x, y) => y == 0 ? null : (long?)x % y,
                [SyntaxKind.SlashToken] = (x, y) => y == 0 ? null : (long?)(x / y),
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
