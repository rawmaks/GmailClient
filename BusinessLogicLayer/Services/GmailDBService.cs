using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Services
{
    public class GmailDBService : IMailService
    {
        private GmailAPIService _apiService;

        public GmailDBService()
        {
            _apiService = GmailAPIService.Instance;
        }


        public IEnumerable<MessageDTO> GetMessagesAsync()
        {
            List<MessageDTO> result = new List<MessageDTO>();

            List<Message> inboxMessages = _apiService.GetMessages(new string[] { "INBOX" });

            if (inboxMessages?.Count > 0)
            {
                foreach (Message message in inboxMessages)
                {
                    result.Add(MessageHelper.ConvertToCorrectType(_apiService.GetMessageByID(message.Id)));
                    break;
                }
            }

            return result;
        }
    }
}
