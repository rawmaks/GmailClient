using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Infrastructure.Enums;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLayer.Helpers
{
    public class MessageHelper
    {
        // Singleton
        public static MessageHelper Instance { get; } = new MessageHelper();

        // Dependencies
        private readonly Base64Helper _base64Helper;
        private readonly DateTimeHelper _dateTimeHelper;

        public MessageHelper()
        {
            _base64Helper = Base64Helper.Instance;
            _dateTimeHelper = DateTimeHelper.Instance;
        }

        public MessageDTO ConvertToCorrectType(Message message)
        {
            if (message == null) return null;
            
            try
            {
                MessageDTO result = new MessageDTO();
                result.CreateDate = DateTime.Now; // TODO: !!!

                /// HEADERS
                result.Subject = message?.Payload?.Headers?.FirstOrDefault(m => m.Name.ToLower() == "Subject".ToLower())?.Value;
                result.Sender = message?.Payload?.Headers?.FirstOrDefault(m => m.Name.ToLower() == "From".ToLower())?.Value;
                result.RecipientEmail = message?.Payload?.Headers?.FirstOrDefault(m => m.Name.ToLower() == "To".ToLower())?.Value;
                //result.Date = _dateTimeHelper.ConvertFromGmailFormat(message?.Payload?.Headers?.FirstOrDefault(m => m.Name.ToLower() == "Date".ToLower())?.Value);
                result.Date = _dateTimeHelper.ConvertFromGmailFormat(message?.InternalDate);

                if (string.IsNullOrWhiteSpace(result.Subject)) result.Subject = "Без темы";

                if (!string.IsNullOrWhiteSpace(result.Sender))
                {
                    if (result.Sender.Contains("<") && result.Sender.Contains(">"))
                    {
                        result.SenderName = result.Sender.Substring(0, result.Sender.IndexOf("<")).Trim();
                        result.SenderEmail = result.Sender.Substring(result.Sender.IndexOf("<") + 1, (result.Sender.IndexOf(">") - result.Sender.IndexOf("<") - 1)).Trim();
                        result.Sender = result.Sender.Replace("<", "(").Replace(">", ")").Trim();
                    }
                    else result.SenderEmail = result.Sender;
                }

                /// BODY
                if (message.Payload?.Parts != null)
                {
                    foreach (var part in message.Payload.Parts)
                    {
                        if (part.MimeType == "text/html")
                        {
                            result.Body = _base64Helper.Decode(part.Body?.Data);
                        }
                    }
                }
                else result.Body = _base64Helper.Decode(message?.Payload?.Body?.Data);

                /// OTHER FIELDS
                result.MessageID = message.Id;
                result.MessageTypeID = (int)GetMessageType(result.Body);
                result.StatusID = (int)GetStatus(result.MessageTypeID);

                return result;
            }
            catch (Exception ex)
            {
                // TODO: ILoggerFactory
                return null;
            }

        }

        public MessageTypes GetMessageType(string body)
        {
            if (body == null) return MessageTypes.None;

            try
            {
                if (body.Contains("бро")) return MessageTypes.InboxSpeaker;
                else return MessageTypes.InboxNone;
            }
            catch
            {
                // TODO: ILoggerFactory
                return MessageTypes.None;
            }
        }

        public MessageStatuses GetStatus(int messageType)
        {
            if (messageType == 0 || messageType == (int)MessageTypes.None) return MessageStatuses.None;

            try
            {
                if (messageType == (int)MessageTypes.InboxSpeaker) return MessageStatuses.New;
                else return MessageStatuses.OtherMail;
            }
            catch
            {
                // TODO: ILoggerFactory
                return MessageStatuses.None;
            }
        }


    }
}
