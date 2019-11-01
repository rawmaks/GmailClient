using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Helpers
{
    public class DateTimeHelper
    {
        // Singleton
        public static DateTimeHelper Instance { get; } = new DateTimeHelper();

        public DateTime ConvertFromGmailFormat(string dateTime)
        {
            return DateTime.Now;
        }
    }
}
