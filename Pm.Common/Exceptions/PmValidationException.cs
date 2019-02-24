using System;

namespace Pm.Common.Exceptions
{
    public class PmValidationException : PmException
    {
        public PmValidationException()
        {
        }

        public PmValidationException(string message) : base(message)
        {
        }

        public PmValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
