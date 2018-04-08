namespace Refactoring.Helper
{
    internal enum RefactoringId
    {
        BooleanConstantComparison = 1,
        BooleanConstantSimplifier,
        ConditionalComplexity,
        DepthOfInheritance,
        IfAndElseBlockEquals,
        IfReturnBoolean,
        IllegalFieldAccess,
        IntegerConstantSimplifier,
        LackOfCohesion,
        LinesOfCode,
        NotOperatorInversion,
        LongParameterList,
        PotentialStaticMethod,
        IdentifierConvention
    }

    internal static class RefactoringIdsMethods
    {
        public static string GetDiagnosticId(this RefactoringId @this)
        {
            const int diagnosticIdPadLength = 3;
            return "SASKIA" + ((int) @this).ToString().PadLeft(diagnosticIdPadLength, '0');
        }
    }
}