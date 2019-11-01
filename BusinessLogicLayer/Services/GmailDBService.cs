using DataAccessLayer.Interfaces;
using DALEntities = DataAccessLayer.Entities;

using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;

using Google.Apis.Gmail.v1;
using GmailData = Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace BusinessLogicLayer.Services
{
    public class GmailDBService : IMailService
    {
        // Dependencies
        private readonly IUnitOfWork _database;

        private readonly GmailAPIService _apiService;
        private readonly MessageHelper _messageHelper;

        public GmailDBService(IUnitOfWork uow)
        {
            _database = uow;
            _apiService = GmailAPIService.Instance;
            _messageHelper = MessageHelper.Instance;
        }


        public void InitializeMessages()
        {
            // Получение списка ВХОДЯЩИХ сообщений из Gmail API
            List<GmailData.Message> inboxMessages = _apiService.GetMessages(new string[] { "INBOX" });

            if (inboxMessages?.Count > 0)
            {
                foreach (GmailData.Message message in inboxMessages)
                {
                    if (!(_database.Messages.Any(m => m.MessageID == message.Id)))
                    {
                        // TODO: !!!
                        MessageHelper messageHelper = new MessageHelper();

                        MessageDTO messageDTO = messageHelper.ConvertToCorrectType(_apiService.GetMessageByID(message.Id));

                        if (messageDTO != null)
                        {
                            _database.Messages.Create(GetMessageDTOToMessageMapper().Map<MessageDTO, DALEntities.Message>(messageDTO));
                        }
                    }
                }

            }
        }

        // TODO: Return result
        public async Task InitializeMessagesAsync()
        {
            // Получение списка ВХОДЯЩИХ сообщений из Gmail API
            List<GmailData.Message> inboxMessages = await _apiService.GetMessagesAsync(new string[] { "INBOX" });

            if (inboxMessages?.Count > 0)
            {
                foreach (GmailData.Message message in inboxMessages)
                {
                    if (!(await _database.Messages.AnyAsync(m => m.MessageID == message.Id)))
                    {
                        // TODO: !!!
                        MessageHelper messageHelper = new MessageHelper();

                        MessageDTO messageDTO = messageHelper.ConvertToCorrectType(await _apiService.GetMessageByIDAsync(message.Id));

                        if (messageDTO != null)
                        {
                            await _database.Messages.CreateAsync(GetMessageDTOToMessageMapper().Map<MessageDTO, DALEntities.Message>(messageDTO));
                        }
                    }
                }

            }
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync()
        {
            return GetMessageToMessageDTOMapper().Map<IEnumerable<DALEntities.Message>, List<MessageDTO>>(await _database.Messages.GetListAsync());
        }

        public IEnumerable<MessageDTO> GetMessages()
        {
            return GetMessageToMessageDTOMapper().Map<IEnumerable<DALEntities.Message>, List<MessageDTO>>(_database.Messages.GetList());
        }


        // TODO: Move to Mappers
        private IMapper GetMessageToMessageDTOMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.CreateMap<DALEntities.MessageType, MessageTypeDTO>();
                cfg.CreateMap<DALEntities.Message, MessageDTO>();
            }).CreateMapper();
        }

        private IMapper GetMessageDTOToMessageMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.CreateMap<MessageTypeDTO, DALEntities.MessageType>();
                cfg.CreateMap<MessageDTO, DALEntities.Message>();
            }).CreateMapper();
        }

    }
}
