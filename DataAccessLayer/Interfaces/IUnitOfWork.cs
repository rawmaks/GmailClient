using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork// : IDisposable
    {
        IRepository<Settings> Settings { get; }
        IMessageRepository<Message> Messages { get; }
        IMessageRepository<MessageType> MessageTypes { get; }
        IMessageRepository<ParticipantMessage> ParticipantMessages { get; }
        //Task SaveAsync();
    }
}
