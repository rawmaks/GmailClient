using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Helpers
{
    public class DateTimeHelper
    {
        // Singleton
        public static DateTimeHelper Instance { get; } = new DateTimeHelper();

        public DateTime? ConvertFromGmailFormat(string dateTime)
        {
            if (dateTime == null) return null;

            return DateTime.Now;
        }

        public DateTime? ConvertFromGmailFormat(long? dateTime)
        {
            if (dateTime == null) return null;

            return TimeZoneInfo.ConvertTime(DateTimeOffset.FromUnixTimeMilliseconds(dateTime.Value).DateTime, TimeZoneInfo.Local);
        }
    }
}
