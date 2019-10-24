using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class Message : BaseEntity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime Date { get; set; }
        public bool IsResponseSent { get; set; }
        public string Subject { get; set; }
        public MessageBody BodyFields { get; set; }
        public string BodyRaw { get; set; }
    }
}
