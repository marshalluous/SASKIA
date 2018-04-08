namespace Refactoring.Helper
{
    internal static class RefactoringMessageFactory
    {
        /* BooleanConstantComparison */
        public static string BooleanConstantComparisonTitle() =>
            "Boolean constant comparison";

        public static string BooleanConstantComparisonDescription() =>
            "Simplify boolean comparison expression";

        public static string BooleanConstantComparisonMessage(bool comparisonLiteral) =>
            $"Unnecessary comparison with {BooleanToString(comparisonLiteral)} detected";

        /* BooleanConstantSimplifier */
        public static string BooleanConstantSimplifierTitle() =>
            "Boolean constant simplifier";

        public static string BooleanConstantSimplifierDescription() =>
            "Simplify boolean literal expression";

        public static string BooleanConstantSimplifierMessage(bool simplifiedValue) =>
             $"Constant boolean expression can be simplified to {BooleanToString(simplifiedValue)}";

        private static string BooleanToString(bool boolValue) =>
            boolValue ? "true" : "false";

        /* ConditionalComplexity */
        public static string ConditionalComplexityTitle() =>
            "Conditional Complexity";

        public static string ConditionalComplexityMessage(int mcCabeComplexity) =>
            $"Method code is too complex (McCabe complexity = {mcCabeComplexity}";

        /* DepthOfInheritance */
        public static string DepthOfInheritanceTitle() =>
            "Depth Of Inheritance";

        public static string DepthOfInheritanceMessage(string className, int levels) =>
            $"The class {className} has {levels} inheritance levels";
    }
}
