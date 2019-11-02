using AutoMapper;
using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Interfaces;
using PresentationLayer.Model;
using PresentationLayer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            set { title = value; OnPropertyChanged("Title"); }
        }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged("Messages"); }
        }


        private RelayCommand close;
        public RelayCommand Close
        {
            get
            {
                return close ??
                  (close = new RelayCommand(obj =>
                  {
                      Window mailView = obj as MailView;
                      mailView.Close();
                  }));
            }
        }

        private RelayCommand minimize;
        public RelayCommand Minimize
        {
            get
            {
                return minimize ??
                  (minimize = new RelayCommand(obj =>
                  {
                      Window mailView = obj as MailView;
                      mailView.WindowState = WindowState.Minimized;
                  }));
            }
        }

        private RelayCommand dragMove;
        public RelayCommand DragMove
        {
            get
            {
                return dragMove ??
                  (dragMove = new RelayCommand(obj =>
                  {
                      Window mailView = obj as MailView;
                      mailView.DragMove();
                  }));
            }
        }

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




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
