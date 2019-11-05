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
using System.Timers;
using System.Windows;

namespace PresentationLayer.ViewModels
{
    public class MailViewModel : INotifyPropertyChanged
    {
        // Dependencies
        private readonly IMailService _mailService;
        private readonly Mappers _mappers;

        private readonly Timer _timer;

        public MailViewModel(IMailService mailService)
        {
            _mailService = mailService;
            _mappers = Mappers.Instance;

            _timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Stop(); // То же, что и _timer.Enabled = false;

            Task.Run(async () => await Load()); //Task.Run(() => { });
        }


        public async Task Load()
        {
            await RefreshTitles();

            await RefreshMessageTypesList();
            await RefreshList();

            await Reload();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e) => Reload().Wait();
        public async Task Reload()
        {
            _timer.Stop();

            if (IsAutomaticResponderEnabled)
            {
                await _mailService.InitializeAsync();
                await _mailService.CheckResponsesAsync();
                await _mailService.SendResponsesAsync();
                await RefreshList();
            }

            _timer.Start();
        }


        public async Task RefreshList()
        {
            Messages = new ObservableCollection<Message>(_mappers.GetMessageDTOToMessageMapper().Map<IEnumerable<MessageDTO>, List<Message>>(await _mailService.GetMessagesAsync()));
            Title = "";
        }

        public async Task RefreshMessageTypesList()
        {
            List<MessageType> messageTypes = _mappers.GetMessageTypeDTOToMessageTypeMapper().Map<IEnumerable<MessageTypeDTO>, List<MessageType>>(await _mailService.GetMessageTypesAsync());
            messageTypes.FirstOrDefault(m => m.ID == 3).Text = "Все входящие";
            MessageTypes = new ObservableCollection<MessageType>(messageTypes.Where(m => m.ID > 2));
        }

        public async Task RefreshTitles()
        {
            IsAutomaticResponderEnabled = true;
            UserEmail = _mappers.GetUserDTOToUserMapper().Map<UserDTO, User>(await _mailService.GetCurrentUserAsync()).Email;
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
        public RelayCommand FilterListByType => new RelayCommand(obj =>
        {
            MessageType type = obj as MessageType;
            if (type != null && Messages != null)
            {
                
            }
        });


        public bool IsBeingRefreshed { get; set; } // TODO: ???

        private bool isAutomaticResponderEnabled;
        public bool IsAutomaticResponderEnabled
        {
            get { return isAutomaticResponderEnabled; }
            set { isAutomaticResponderEnabled = value; OnPropertyChanged(nameof(IsAutomaticResponderEnabled)); }
        }

        private string title;
        public string Title
        {
            get { return $"Входящие ({(Messages?.Count > 0 ? Messages.Count : default)})"; }
            set { title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string userEmail;
        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; OnPropertyChanged(nameof(UserEmail)); }
        }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(nameof(Messages)); }
        }

        private ObservableCollection<MessageType> messageTypes;
        public ObservableCollection<MessageType> MessageTypes
        {
            get { return messageTypes; }
            set { messageTypes = value; OnPropertyChanged(nameof(MessageTypes)); }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
