using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class Message : BaseEntity
    {
        public string MessageID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Recipient { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public DateTime? Date { get; set; }
        public int StatusID { get; set; }
        public MessageStatus Status { get; set; }
        public int MessageTypeID { get; set; }
        public MessageType MessageType { get; set; }
        public ParticipantMessage Participant { get; set; }
    }
}
