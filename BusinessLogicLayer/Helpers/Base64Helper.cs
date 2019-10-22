using System;
using System.Text;

namespace BusinessLogicLayer.Helpers
{
    public class Base64Helper
    {

        public static string Decode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "<strong>Message body was not returned from Google</strong>";

            string InputStr = input.Replace("-", "+").Replace("_", "/");
            return Encoding.UTF8.GetString(Convert.FromBase64String(InputStr));

        }

    }
}
