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
        private readonly Mappers _mappers;

        public MailViewModel(IMailService mailService)
        {
            _mailService = mailService;
            _mappers = Mappers.Instance;

            Task.Run(async () => await Load()); //Task.Run(() => { });
        }


        public async Task Load()
        {
            await RefreshList();

            await Task.Run(async () => {
                await _mailService.InitializeAsync();
                await RefreshList();
            });

            // TODO: Запустить проверку на отправленные ответы
        }

        public async Task RefreshList()
        {
            Messages = new ObservableCollection<Message>(_mappers.GetMessageDTOToMessageMapper().Map<IEnumerable<MessageDTO>, List<Message>>(await _mailService.GetMessagesAsync()));
            Title = "";
        }


        public RelayCommand Close => new RelayCommand(obj => (obj as MailView).Close());
        public RelayCommand Minimize => new RelayCommand(obj => (obj as MailView).WindowState = WindowState.Minimized);
        public RelayCommand DragMove => new RelayCommand(obj => (obj as MailView).DragMove());
        public RelayCommand OpenSettings => new RelayCommand(obj => (obj as MailView).Close());
        public RelayCommand Refresh => new RelayCommand(obj => Task.Run(async () => await RefreshList()));
        public RelayCommand SendMail => new RelayCommand(obj =>
        {
            int id = (int)obj;
            Message message = Messages.FirstOrDefault(x => x.ID == id);
            if (message != null && message?.StatusID != 2)
            {
                message.StatusID = 2;
            }
        });


        public bool IsBeingRefreshed { get; set; } // TODO: ???

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



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
