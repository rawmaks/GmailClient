using BusinessLogicLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Interfaces
{
    public interface IMailService
    {
        IEnumerable<MessageDTO> GetMessagesAsync();
    }
}
