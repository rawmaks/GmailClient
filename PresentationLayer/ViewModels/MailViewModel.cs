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
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class MailViewModel : INotifyPropertyChanged
    {
        // Dependencies
        private readonly IMailService _mailService;

        public MailViewModel(IMailService mailService)
        {
            _mailService = mailService;

            Task.Run(() => {
                Getcheg();
            });
        }

        public void Getcheg() // async Task
        {
            //await _mailService.InitializeMessagesAsync();
            //_mailService.InitializeMessages();
            _mailService.InitializeMessagesAsync().Wait();

            IEnumerable<MessageDTO> messageDTOs = _mailService.GetMessages(); // await

            IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageTypeDTO, MessageType>();
                cfg.CreateMap<MessageDTO, Message>();
            }).CreateMapper();
            List<Message> messages = mapper.Map<IEnumerable<MessageDTO>, List<Message>>(messageDTOs);


            Messages = new ObservableCollection<Message>(messages);
            Title = "";
        }

        private string title;
        public string Title
        {
            get { return $"Входящие ({(Messages?.Count > 0 ? Messages.Count : default)})"; }
            set { title = value; OnPropertyChanged("Title"); }
        }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged("Messages"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
