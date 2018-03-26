namespace Refactoring
{
    public sealed class DiagnosticInfo
    {
        private DiagnosticInfo()
        {
        }

        public bool DiagnosticFound { get; private set; }
        public string Message { get; private set; }
        public object AdditionalInformation { get; private set; }

        public static DiagnosticInfo CreateSuccessfulResult(object additionalInformation = null)
        {
            return new DiagnosticInfo
            {
                AdditionalInformation = additionalInformation
            };
        }

        public static DiagnosticInfo CreateFailedResult(string message, object additionalInformation = null)
        {
            return new DiagnosticInfo
            {
                DiagnosticFound = true,
                Message = message,
                AdditionalInformation = additionalInformation
            };
        }
    }
}