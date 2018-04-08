namespace Refactoring.Helper
{
    internal static class RefactoringMessageFactory
    {
        public static string BooleanComparisonTitle() =>
            "Boolean constant comparison";

        public static string BooleanComparisonDescription() =>
            "Simplify boolean comparison expression";

        public static string BooleanComparisonMessage(string comparisonKeyword) =>
            $"Unnecessary comparison with {comparisonKeyword} detected";
    }
}
