using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class ParticipantMessage : BaseEntity
    {
        public int MessageID { get; set; }
        public Message Message { get; set; }
        public string FirstName { get; set; }
        // TODO: ...
    }
}
