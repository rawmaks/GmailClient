using AutoMapper;
using BusinessLogicLayer.DataTransferObjects;
using PresentationLayer.Model.Entities;

namespace PresentationLayer.Model
{
    public class Mappers
    {
        // Singleton
        public static Mappers Instance { get; } = new Mappers();

        public IMapper GetMessageDTOToMessageMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageTypeDTO, MessageType>();
                cfg.CreateMap<MessageDTO, Message>();
            }).CreateMapper();
        }

        public IMapper GetMessageTypeDTOToMessageTypeMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageTypeDTO, MessageType>();
            }).CreateMapper();
        }


        public IMapper GetUserDTOToUserMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>();
            }).CreateMapper();
        }



    }
}
