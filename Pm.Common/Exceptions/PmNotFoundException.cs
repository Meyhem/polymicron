using System;

namespace Pm.Common.Exceptions
{
    public class PmNotFoundException : PmException
    {
        public PmNotFoundException()
        {
        }

        public PmNotFoundException(string message) : base(message)
        {
        }

        public PmNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
