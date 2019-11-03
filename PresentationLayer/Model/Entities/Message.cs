using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PresentationLayer.Model.Entities
{
    public class Message : BaseEntity, INotifyPropertyChanged
    {
        private string messageID;
        public string MessageID
    {
            get { return messageID; }
            set { messageID = value; OnPropertyChanged(nameof(MessageID)); }
        }

        private string subject;
        public string Subject
        {
            get { return subject; }
            set { subject = value; OnPropertyChanged(nameof(Subject)); }
        }

        private string sender;
        public string Sender
        {
            get { return sender; }
            set { sender = value; OnPropertyChanged(nameof(Sender)); }
        }

        private string senderName;
        public string SenderName
        {
            get { return senderName; }
            set { senderName = value; OnPropertyChanged(nameof(SenderName)); }
        }

        private string senderEmail;
        public string SenderEmail
        {
            get { return senderEmail; }
            set { senderEmail = value; OnPropertyChanged(nameof(SenderEmail)); }
        }

        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged(nameof(Date)); }
        }

        private int statusID;
        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; OnPropertyChanged(nameof(StatusID)); }
        }

        /// <summary>
        /// Тип письма
        /// </summary>
        private int messageTypeID;
        public int MessageTypeID
        {
            get { return messageTypeID; }
            set { messageTypeID = value; OnPropertyChanged(nameof(MessageTypeID)); }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}