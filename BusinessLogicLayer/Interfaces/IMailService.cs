using BusinessLogicLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IMailService
    {
        Task InitializeAsync();
        Task CheckResponsesAsync();
        Task SendResponsesAsync();
        Task<UserDTO> GetCurrentUserAsync();
        Task<IEnumerable<MessageDTO>> GetMessagesAsync();
        Task<IEnumerable<MessageTypeDTO>> GetMessageTypesAsync();
    }
}
