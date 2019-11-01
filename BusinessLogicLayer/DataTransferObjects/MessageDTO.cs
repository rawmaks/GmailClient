﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DataTransferObjects
{
    public class MessageDTO : BaseEntityDTO
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

        /// <summary>
        /// Был ли отправлен ответ?
        /// </summary>
        public bool IsResponseSent { get; set; }
        public int MessageTypeID { get; set; }
        public MessageTypeDTO MessageType { get; set; }
        public ParticipantMessageDTO Participant { get; set; }
    }
}
