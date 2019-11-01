using BusinessLogicLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IMailService
    {
        void InitializeMessages();
        IEnumerable<MessageDTO> GetMessages();
        Task InitializeMessagesAsync();
        Task<IEnumerable<MessageDTO>> GetMessagesAsync();
    }
}
