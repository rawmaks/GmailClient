using AutoMapper;
using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Interfaces;
using PresentationLayer.Infrastructure.Enums;
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
using System.Windows.Data;
using Enums = PresentationLayer.Infrastructure.Enums;

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
            await Refresh();
            await CheckUpdate();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e) => CheckUpdate().Wait();
        public async Task CheckUpdate()
        {
            _timer.Stop();

            await _mailService.InitializeAsync();
            await _mailService.CheckResponsesAsync();
            if (IsAutomaticResponderEnabled) await _mailService.SendResponsesAsync();
            await GetMessageList();

            _timer.Start();
        }

        public async Task Refresh()
        {
            IsAutomaticResponderEnabled = true; // TODO: Get from AppSettings in DB

            await GetUser();
            
            await GetMessageTypesList();
            SelectedLeftMenuItemIndex = SelectedLeftMenuItemIndex; // TODO: Get from AppSettings in DB

            await GetMessageList();
        }

        public async Task GetMessageList()
        {
            Messages = new CollectionView(_mappers.GetMessageDTOToMessageMapper().Map<IEnumerable<MessageDTO>, List<Message>>(await _mailService.GetMessagesAsync()));
            IsSortByAsc = !IsSortByAsc;//true; // TODO: Get from AppSettings in DB
            SortListByType();
            // TODO: Filter
            Title = default;
        }

        public async Task GetMessageTypesList()
        {
            List<MessageType> messageTypes = _mappers.GetMessageTypeDTOToMessageTypeMapper().Map<IEnumerable<MessageTypeDTO>, List<MessageType>>(await _mailService.GetMessageTypesAsync());
            messageTypes.FirstOrDefault(m => m.ID == (int)Enums.MessageTypes.Inbox).Text = "Все входящие";
            MessageTypes = new CollectionView(messageTypes);

            ICollectionView filterView = CollectionViewSource.GetDefaultView(MessageTypes.SourceCollection.Cast<MessageType>());
            filterView.Filter = item =>
            {
                MessageType messageType = item as MessageType;
                return messageType != null && messageType.ID > (int)Enums.MessageTypes.None && messageType.ID != (int)Enums.MessageTypes.InboxNone;
            };
            MessageTypes = filterView;
        }

        public async Task GetUser()
        {
            UserEmail = _mappers.GetUserDTOToUserMapper().Map<UserDTO, User>(await _mailService.GetCurrentUserAsync()).Email;
        }


        public RelayCommand Close => new RelayCommand(obj => (obj as MailView).Close());
        public RelayCommand Minimize => new RelayCommand(obj => (obj as MailView).WindowState = WindowState.Minimized);
        public RelayCommand DragMove => new RelayCommand(obj => (obj as MailView).DragMove());
        public RelayCommand OpenSettings => new RelayCommand(obj => (obj as MailView).Close());
        public RelayCommand RefreshView => new RelayCommand(obj => Task.Run(async () => await Refresh()));
        public RelayCommand SortList => new RelayCommand(obj => SortListByType(obj as string));
        public RelayCommand FilterList => new RelayCommand(obj =>
        {
            MessageType type = obj as MessageType;
            if (type != null && Messages != null)
            {
                SelectedLeftMenuItemIndex = MessageTypes.Cast<MessageType>().ToList().IndexOf(type); // TODO: Write to DB (AppSettings)
                FilterListByType(type);
            }
        });
        public RelayCommand SendMail => new RelayCommand(obj =>
        {
            int id = (int)obj;
            Message message = Messages.SourceCollection.Cast<Message>().FirstOrDefault(x => x.ID == id);
            if (message != null && message?.StatusID != (int)MessageStatuses.OtherMail && message?.StatusID != (int)MessageStatuses.Success)
            {
                message.StatusID = new Random().Next(2, 6);
            }
        });


        // TODO: Это конечно нужно будет переделать
        private void SortListByType(string type = "Date")
        {
            ICollectionView sortView = CollectionViewSource.GetDefaultView(Messages.SourceCollection.Cast<Message>());
            sortView.SortDescriptions.Clear();
            if (IsSortByAsc) { sortView.SortDescriptions.Add(new SortDescription(type ?? "Date", ListSortDirection.Descending)); IsSortByAsc = false; }
            else { sortView.SortDescriptions.Add(new SortDescription(type ?? "Date", ListSortDirection.Ascending)); IsSortByAsc = true; }
            Messages = sortView;
            Title = default;
        }

        // TODO: Это конечно нужно будет переделать
        private void FilterListByType(MessageType type)
        {
            List<int> messageTypeIDs = new List<int>();
            Recursion(type);
            void Recursion(MessageType type)
            {
                List<MessageType> children = MessageTypes.SourceCollection.Cast<MessageType>().ToList().Where(m => m.ParentID == type.ID).ToList();

                if (children?.Count > 0)
                {
                    foreach (MessageType child in MessageTypes.SourceCollection.Cast<MessageType>().ToList().Where(m => m.ParentID == type.ID))
                    {
                        messageTypeIDs.Add(child.ID);
                        Recursion(child);
                    }
                }
                else if (!messageTypeIDs.Any(m => m == type.ID)) messageTypeIDs.Add(type.ID);
            }
            
            ICollectionView filterView = CollectionViewSource.GetDefaultView(Messages.SourceCollection.Cast<Message>());
            filterView.Filter = item =>
            {
                Message message = item as Message;
                return message != null && messageTypeIDs.Any(m => m == message?.MessageTypeID);
            };
            Messages = filterView;
            Title = default;
        }


        public bool IsBeingRefreshed { get; set; } // TODO: ???
        public bool IsSortByAsc { get; set; }

        private bool isAutomaticResponderEnabled;
        public bool IsAutomaticResponderEnabled
        {
            get { return isAutomaticResponderEnabled; }
            set { isAutomaticResponderEnabled = value; OnPropertyChanged(nameof(IsAutomaticResponderEnabled)); }
        }

        private int selectedLeftMenuItemIndex;
        public int SelectedLeftMenuItemIndex
        {
            get { return selectedLeftMenuItemIndex; }
            set { selectedLeftMenuItemIndex = value; OnPropertyChanged(nameof(SelectedLeftMenuItemIndex)); }
        }

        private string title;
        public string Title
        {
            get { return $"Входящие ({(Messages?.Cast<Message>().Count() > 0 ? Messages.Cast<Message>().Count() : default )})"; }
            set { title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string userEmail;
        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; OnPropertyChanged(nameof(UserEmail)); }
        }

        private ICollectionView messages;
        public ICollectionView Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(nameof(Messages)); }
        }

        private ICollectionView messageTypes;
        public ICollectionView MessageTypes
        {
            get { return messageTypes; }
            set { messageTypes = value; OnPropertyChanged(nameof(MessageTypes)); }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
