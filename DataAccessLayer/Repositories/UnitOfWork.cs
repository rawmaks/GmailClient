using DataAccessLayer.DBContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //protected readonly MessageContext _dbContext;
        protected readonly DbContextOptions<MessageContext> _options;

        protected IRepository<Settings> _settingsRepository;
        protected IMessageRepository<Message> _messageRepository;
        protected IMessageRepository<MessageType> _messageTypeRepository;

        public UnitOfWork(string connectionString)
        {
            var options = new DbContextOptionsBuilder<MessageContext>()
                    .UseSqlServer(connectionString)
                    .Options;

            //_dbContext = new MessageContext(options);
            _options = options;
        }


        public IRepository<Settings> Settings
        {
            get
            {
                if (_settingsRepository == null)
                    _settingsRepository = new BaseRepository<Settings>(_options);
                return _settingsRepository;
            }
        }

        public IMessageRepository<Message> Messages
        {
            get
            {
                if (_messageRepository == null)
                    _messageRepository = new MessageRepository<Message>(_options);
                return _messageRepository;
            }
        }

        public IMessageRepository<MessageType> MessageTypes
        {
            get
            {
                if (_messageTypeRepository == null)
                    _messageTypeRepository = new MessageRepository<MessageType>(_options);
                return _messageTypeRepository;
            }
        }




        //private bool disposed = false;

        //public virtual void Dispose(bool disposing)
        //{
        //    if (!disposed)
        //    {
        //        if (disposing)
        //        {
        //            _dbContext.Dispose();
        //        }
        //        disposed = true;
        //    }
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //public async Task SaveAsync()
        //{
        //    await _dbContext.SaveChangesAsync();
        //}
    }
}
