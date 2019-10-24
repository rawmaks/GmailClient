using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DataTransferObjects
{
    public class MessageDTO
    {
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Recipient { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmail { get; set; }
        public string Date { get; set; } // !!!
        public bool IsResponseSent { get; set; }
        public string Subject { get; set; }
        public MessageBodyDTO BodyFields { get; set; }
        public string BodyRaw { get; set; }
    }
}
