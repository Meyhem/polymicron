using System;
using System.Collections.Generic;
using System.Text;

namespace Pm.Common.Exceptions
{
    public static class PmAssert
    {
        public static void AssertOrNotFound(bool assertion, string message = null)
        {
            if (!assertion)
            {
                throw new PmNotFoundException(message);
            }
        }

        public static void AssertOrValidationError(bool assertion, string message = null)
        {
            if (!assertion)
            {
                throw new PmValidationException(message);
            }
        }

    }
}
