using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DataTransferObjects
{
    public class ParticipantMessageDTO : BaseEntityDTO
    {
        public int MessageID { get; set; }
        public MessageDTO Message { get; set; }
        public string FirstName { get; set; }
        // TODO: ...
    }
}
