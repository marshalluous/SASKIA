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
            $"Method code is too complex (McCabe complexity = {mcCabeComplexity})";

        /* DepthOfInheritance */
        public static string DepthOfInheritanceTitle() =>
            "Depth Of Inheritance";

        public static string DepthOfInheritanceMessage(string className, int levels) =>
            $"The class {className} has {levels} inheritance levels";

        /* DeMorganSimplifier */
        public static string DeMorganSimplifierTitle() =>
            "Negation of && or || expression can be simplified";

        public static string DeMorganSimplifierDescription() =>
            DeMorganSimplifierTitle();

        public static string DeMorganSimplifierMessage(string actualExpression) =>
            $"{actualExpression} can be simplified";

        /* IfAndElseBlockEquals */
        public static string IfAndElseBlockEqualsTitle() =>
            "If and else block are identical";

        public static string IfAndElseBlockEqualsDescription() =>
            "Get rid of the if statement";

        public static string IfAndElseBlockEqualsMessage() =>
            "If statement is not needed because then and else block are identical";

        /* IfReturnBoolean */
        public static string IfReturnBooleanTitle() =>
            "If and else block just returns condition";

        public static string IfReturnBooleanDescription() =>
            "Get rid of the if statement";

        public static string IfReturnBooleanMessage(string expression) =>
            $"If statement is not needed. Just write {expression}";

        /* IllegalFieldAccess */
        public static string IllegalFieldAccessTitle() =>
            "Non-private field detected";

        public static string IllegalFieldAccessDescription() =>
            "Make field private";

        public static string IllegalFieldAccessMessage() =>
            "Field should be private";

        /* IntegerConstantSimplifier */
        public static string IntegerConstantSimplifierTitle() =>
            "Integer constant simplifier";

        public static string IntegerConstantSimplifierDescription() =>
            "Simplify integer literal expression";

        public static string IntegerConstantSimplifierMessage(int simplifiedValue) =>
             $"Constant integer expression can be simplified to {simplifiedValue}";

        /* LongConstantSimplifier */
        public static string LongConstantSimplifierTitle() =>
            "Long constant simplifier";

        public static string LongConstantSimplifierDescription() =>
            "Simplify long literal expression";

        public static string LongConstantSimplifierMessage(long simplifiedValue) =>
            $"Constant long expression can be simplified to {simplifiedValue}";

        /* LongParameterList */
        public static string LongParameterListTitle() =>
            "Long parameter list detected";

        public static string LongParameterListDescription() =>
            "Long parameter list detected";

        public static string LongParameterListMessage(int parameterCount) =>
            $"Long parameter with {parameterCount} parameters list detected";

        /* LackOfCohesion */
        public static string LackOfCohesionTitle() =>
            "Weak cohesion in class";

        public static string LackOfCohesionMessage(double lcomValue) =>
            $"Weak cohesion in class detected (LCOM*={lcomValue})";

        /* TypeIdentifier */
        public static string TypeIdentifierTitle() =>
            "Unconventional type name";

        public static string TypeIdentifierDescription() =>
            "Refactor type name to upper camel case";

        public static string TypeIdentifierMessage(string actualTypeName, string refactoredTypeName) =>
            $"Refactor type name from {actualTypeName} to {refactoredTypeName}";

        /* UseOfVarRefactoring */
        public static string UseOfVarTitle() =>
            "Use var instead";

        public static string UseOfVarDescription() =>
            "Use var instead";

        public static string UseOfVarMessage(string typeName) =>
            $"Use var instead of {typeName}";

        /* WhitespaceFixRefactoring */
        public static string WhitespaceFixTitle() =>
            "Unconventional code formatting";

        public static string WhitespaceFixDescription() =>
            "Format code";

        public static string WhitespaceFixMessage() =>
            "Correct formatted code makes the source code easier to read";
    }
}