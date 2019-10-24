using BusinessLogicLayer.DataTransferObjects;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLayer.Helpers
{
    public class MessageHelper
    {
        // Converter
        // Verify

        public static MessageDTO ConvertToCorrectType(Message message)
        {
            MessageDTO result = null;


            result = new MessageDTO();

            result.Subject = message?.Payload?.Headers?.FirstOrDefault(m => m.Name.ToLower() == "Subject".ToLower())?.Value;
            result.Sender = message?.Payload?.Headers?.FirstOrDefault(m => m.Name.ToLower() == "From".ToLower())?.Value;
            result.RecipientEmail = message?.Payload?.Headers?.FirstOrDefault(m => m.Name.ToLower() == "To".ToLower())?.Value;
            result.Date = message?.Payload?.Headers?.FirstOrDefault(m => m.Name.ToLower() == "Date".ToLower())?.Value;

            if (!string.IsNullOrWhiteSpace(result.Sender) && result.Sender.Contains("<") && result.Sender.Contains(">"))
            {
                result.SenderName = result.Sender.Substring(0, result.Sender.IndexOf("<")).Trim();
                result.SenderEmail = result.Sender.Substring(result.Sender.IndexOf("<") + 1, (result.Sender.IndexOf(">") - result.Sender.IndexOf("<") - 1)).Trim();
                result.Sender = result.Sender.Replace("<", "(").Replace(">", ")").Trim();
            }

            if (message.Payload.Parts != null)
            {
                foreach (var part in message.Payload.Parts)
                {
                    if (part.MimeType == "text/html")
                    {
                        // TODO: Helper
                        result.BodyRaw = Base64Helper.Decode(part.Body?.Data);
                    }
                }
            }
            else
            {
                // TODO: Helper
                result.BodyRaw = Base64Helper.Decode(message?.Payload?.Body?.Data);
            }

            return result;
        }
    }
}
