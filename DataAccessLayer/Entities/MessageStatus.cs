using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class MessageStatus : BaseEntity
    {
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
    }
}
