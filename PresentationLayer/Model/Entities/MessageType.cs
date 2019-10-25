using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer.Model.Entities
{
    public class MessageType : BaseEntity
    {
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
