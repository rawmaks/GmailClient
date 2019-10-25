using BusinessLogicLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IMailService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync();
    }
}
