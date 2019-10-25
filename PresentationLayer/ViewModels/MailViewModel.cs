using AutoMapper;
using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Interfaces;

using PresentationLayer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PresentationLayer.ViewModels
{
    public class MailViewModel : INotifyPropertyChanged
    {
        //public MailViewModel(
        //    ISettingsService settingsService,
        //    IMailService mailService,
        //    IDocxService docxService,
        //    IXlsxService xlsxService)
        public MailViewModel(IMailService mailService)
        {
            mailService.GetMessagesAsync();
            //IEnumerable<MessageDTO> messageDTOs = mailService.GetMessagesAsync();

            //IMapper mapper = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<MessageTypeDTO, MessageType>();
            //    cfg.CreateMap<MessageDTO, Message>();
            //}).CreateMapper();
            //List<Message> messages = mapper.Map<IEnumerable<MessageDTO>, List<Message>>(messageDTOs);


            //Messages = new ObservableCollection<Message>(messages);

        }

        public string Title
        {
            // TODO: Нормальный заголовок
            get { return $"Входящие ({(Messages?.Count > 0 ? Messages.Count : default)})"; }
        }
        public ObservableCollection<Message> Messages { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
