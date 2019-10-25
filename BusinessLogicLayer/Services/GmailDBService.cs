using DataAccessLayer.Interfaces;

using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;

using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class GmailDBService : IMailService
    {
        private readonly IUnitOfWork _database;
        private GmailAPIService _apiService; // TODO: IAPIService

        public GmailDBService(IUnitOfWork uow)
        {
            _database = uow;
            _apiService = GmailAPIService.Instance;
        }


        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync()
        {
            var bb = await _database.MessageTypes.GetListAsync();

            IEnumerable<DataAccessLayer.Entities.MessageType> sdf = bb;


            List<MessageDTO> result = new List<MessageDTO>();

            List<Message> inboxMessages = _apiService.GetMessages(new string[] { "INBOX" });

            if (inboxMessages?.Count > 0)
            {
                foreach (Message message in inboxMessages)
                {
                    MessageDTO messageDTO = MessageHelper.ConvertToCorrectType(_apiService.GetMessageByID(message.Id));
                    if (messageDTO != null) result.Add(messageDTO);
                    break;
                }
            }

            return result;
        }
    }
}
