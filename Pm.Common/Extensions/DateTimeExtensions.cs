using System;

namespace Pm.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToPmFormat(this DateTime? self)
        {
            if (!self.HasValue) return "";
            return ToPmFormat(self.Value);
        }

        public static string ToPmFormat(this DateTime self)
        {
            return self.ToString("dd. MM. yyyy HH:mm:ss");
        }

        public static string ToIso8601(this DateTime? self)
        {
            if (!self.HasValue) return "";
            return self.Value.ToIso8601();
        }

        public static string ToIso8601(this DateTime self)
        {
            return self.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
