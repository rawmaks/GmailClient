using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PresentationLayer.Model.Entities
{
    public class Message : BaseEntity, INotifyPropertyChanged
    {
        private string subject;
        public string Subject
        {
            get { return subject; }
            set { subject = value; OnPropertyChanged("Subject"); }
        }

        private string sender;
        public string Sender
        {
            get { return sender; }
            set { sender = value; OnPropertyChanged("Sender"); }
        }

        private string senderName;
        public string SenderName
        {
            get { return senderName; }
            set { senderName = value; OnPropertyChanged("SenderName"); }
        }

        private string senderEmail;
        public string SenderEmail
        {
            get { return senderEmail; }
            set { senderEmail = value; OnPropertyChanged("SenderEmail"); }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }

        /// <summary>
        /// Был ли отправлен ответ?
        /// </summary>
        private bool isResponseSent;
        public bool IsResponseSent
        {
            get { return isResponseSent; }
            set { isResponseSent = value; OnPropertyChanged("IsResponseSent"); }
        }

        /// <summary>
        /// Тип письма
        /// </summary>
        private MessageType messageType;
        public MessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; OnPropertyChanged("MessageType"); }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}