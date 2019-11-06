using DataAccessLayer.Interfaces;
using DALEntities = DataAccessLayer.Entities;

using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Parsers;
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
        private readonly MessageParser _messageParser;

        public GmailDBService(IUnitOfWork uow)
        {
            _database = uow;
            _apiService = GmailAPIService.Instance;
            _messageHelper = MessageHelper.Instance;
            _messageParser = MessageParser.Instance;
        }


        // TODO: Return result
        public async Task InitializeAsync()
        {
            /// Получение списка ВХОДЯЩИХ сообщений из Gmail API
            List<GmailData.Message> inboxMessages = await _apiService.GetMessagesAsync(new string[] { "INBOX" });

            if (inboxMessages?.Count > 0)
            {
                foreach (GmailData.Message message in inboxMessages)
                {
                    /// Если сообщения нет в базе
                    if (!(await _database.Messages.AnyAsync(m => m.MessageID == message.Id)))
                    {
                        /// Конвертируем сообщение из АПИ в подходящий тип
                        MessageDTO messageDTO = _messageHelper.ConvertToCorrectType(await _apiService.GetMessageByIDAsync(message.Id));

                        if (messageDTO != null)
                        {
                            /// Сохраняем в базу
                            await _database.Messages.CreateAsync(GetMessageDTOToMessageMapper().Map<MessageDTO, DALEntities.Message>(messageDTO));

                            /// Парсим тело сообщения в подходящий тип
                            ParticipantMessageDTO participantMessageDTO = _messageParser.ParseToParticipant(messageDTO.Body);
                            if (participantMessageDTO != null)
                            {
                                /// Получаем идентификатор сохраненного ранее сообщения
                                participantMessageDTO.MessageID = (await _database.Messages.FindAsync(m => m.MessageID == messageDTO.MessageID)).ID;
                                /// Сохраняем распарсенное тело в базу
                                await _database.ParticipantMessages.CreateAsync(GetParticipantMessageDTOToParticipantMessageMapper().Map<ParticipantMessageDTO, DALEntities.ParticipantMessage>(participantMessageDTO));
                            }
                        }
                    }
                }

            }
        }

        public async Task CheckResponsesAsync()
        {
            // TODO: Проверка наличия ответов на новые письма (может быть у входящего письма из апи есть поле, обозначающее, что был отправлен ответ)
        }

        public async Task SendResponsesAsync()
        {
            // TODO: Отправка ответов новым письмам (+ при первом запуске приложения сохранить в настройки дату запуска и с нее отправлять ответы)  //(+ при первой инициализации отправка ответов либо с 1 ноября, либо с запуска приложения, скорее второе)
        }

        public async Task<UserDTO> GetCurrentUserAsync()
        {
            return new UserDTO { Email = (await _apiService.GetProfileAsync()).EmailAddress };
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync()
        {
            return GetMessageToMessageDTOMapper().Map<IEnumerable<DALEntities.Message>, List<MessageDTO>>(await _database.Messages.GetListAsync());
        }

        public async Task<IEnumerable<MessageTypeDTO>> GetMessageTypesAsync()
        {
            return GetMessageTypeToMessageTypeDTOMapper().Map<IEnumerable<DALEntities.MessageType>, List<MessageTypeDTO>>(await _database.MessageTypes.GetListAsync());
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

        private IMapper GetMessageTypeToMessageTypeDTOMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.CreateMap<DALEntities.MessageType, MessageTypeDTO>();
            }).CreateMapper();
        }

        private IMapper GetParticipantMessageDTOToParticipantMessageMapper()
        {
            return new MapperConfiguration(cfg => {
                cfg.CreateMap<ParticipantMessageDTO, DALEntities.ParticipantMessage>();
            }).CreateMapper();
        }


    }
}
