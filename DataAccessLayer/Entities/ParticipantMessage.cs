using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class ParticipantMessage : BaseEntity
    {
        public Message Message { get; set; }
    }
}
