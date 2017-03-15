using System;

namespace Lax.Helpers.DateTimes {

    public static class DateTimeToStringExtensions {

        public static string ToIso8601String(this DateTime value) =>
            value.ToUniversalTime().ToString("o");

    }

}
