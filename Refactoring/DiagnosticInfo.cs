namespace Refactoring
{
    public sealed class DiagnosticInfo
    {
        private DiagnosticInfo()
        {
        }

        public static DiagnosticInfo CreateSuccessfulResult()
        {
            return new DiagnosticInfo();
        }

        public static DiagnosticInfo CreateFailedResult(string message)
        {
            return new DiagnosticInfo
            {
                DiagnosticFound = true,
                Message = message
            };
        }

        public bool DiagnosticFound { get; private set; }
        public string Message { get; private set; }
    }
}