using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class MessageType : BaseEntity
    {
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<Message> Messages { get; set; }
    }
}
