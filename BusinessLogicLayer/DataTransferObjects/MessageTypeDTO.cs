using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DataTransferObjects
{
    public class MessageTypeDTO : BaseEntityDTO
    {
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}
