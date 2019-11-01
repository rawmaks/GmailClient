using System;
using System.Text;

namespace BusinessLogicLayer.Helpers
{
    public class Base64Helper
    {
        // Singleton
        public static Base64Helper Instance { get; } = new Base64Helper();

        // TODO: Исправить комменты
        public string Decode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Зачем мне пустая строка?";

            try
            {
                string result = input.Replace("-", "+").Replace("_", "/");
                result = Encoding.UTF8.GetString(Convert.FromBase64String(result));
                return result;
            }
            catch (Exception ex)
            {
                return $"Не получилось {ex.Message}";
            }

        }

    }
}
