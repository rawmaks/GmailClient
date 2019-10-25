using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DataTransferObjects
{
    public class ParticipantMessageDTO : BaseEntityDTO
    {
        public MessageDTO Message { get; set; }
    }
}
