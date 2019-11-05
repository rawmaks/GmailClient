using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DataTransferObjects
{
    public class MessageStatusDTO : BaseEntityDTO
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}
