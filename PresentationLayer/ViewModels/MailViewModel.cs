using AutoMapper;
using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Interfaces;
using PresentationLayer.Model;
using PresentationLayer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLayer.ViewModels
{
    public class MailViewModel : INotifyPropertyChanged
    {
        // Dependencies
        private readonly IMailService _mailService;

        public MailViewModel(IMailService mailService)
        {
            _mailService = mailService;

            Task.Run(async () => await Load());

            //Task.Run(() => { });
        }



        public async Task Load()
        {
            await RefreshList();

            await Task.Run(async () => {
                await _mailService.InitializeMessagesAsync();
                await RefreshList();
            });

            // TODO: Запустить проверку на отправленные ответы
        }

        public async Task RefreshList()
        {
            IEnumerable<MessageDTO> messageDTOs = await _mailService.GetMessagesAsync();

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
            set { title = value; OnPropertyChanged(nameof(Title)); }
        }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(nameof(Messages)); }
        }


        public RelayCommand Close => new RelayCommand(obj => (obj as MailView).Close());
        public RelayCommand Minimize => new RelayCommand(obj => (obj as MailView).WindowState = WindowState.Minimized);
        public RelayCommand DragMove => new RelayCommand(obj => (obj as MailView).DragMove());


        private RelayCommand openSettings;
        public RelayCommand OpenSettings
        {
            get
            {
                return openSettings ??
                  (openSettings = new RelayCommand(obj =>
                  {
                      
                  }));
            }
        }

        private RelayCommand refresh;
        public RelayCommand Refresh
        {
            get
            {
                return refresh ??
                  (refresh = new RelayCommand(obj =>
                  {

                  }));
            }
        }

        public RelayCommand SendMail => new RelayCommand(obj =>
        {
            int id = (int)obj;
            Messages.FirstOrDefault(x => x.ID == id).StatusID = 2;
        });




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
