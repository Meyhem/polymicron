using System;

namespace Pm.Common.Exceptions
{
    public class PmException : Exception
    {
        public PmException()
        {
        }

        public PmException(string message) : base(message)
        {
        }

        public PmException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
