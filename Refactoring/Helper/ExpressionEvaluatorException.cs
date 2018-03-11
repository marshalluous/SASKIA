using System;

namespace Refactoring.Helper
{
    internal sealed class ExpressionEvaluatorException : Exception
    {
        public ExpressionEvaluatorException()
        {
        }

        public ExpressionEvaluatorException(string message) : base(message)
        {
        }

        public ExpressionEvaluatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
