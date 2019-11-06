using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Enums = DataAccessLayer.Infrastructure.Enums;

namespace DataAccessLayer.DBContext
{
    public class MessageContext: DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<ParticipantMessage> ParticipantMessages { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<MessageStatus> MessageStatuses { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public MessageContext(DbContextOptions<MessageContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageType>().HasData(new List<MessageType>
            {
                new MessageType { ID = (int)Enums.MessageTypes.All, CreateDate = DateTime.Now, ParentID = default, Name = "All", Text = "Все" },

                new MessageType { ID = (int)Enums.MessageTypes.None, CreateDate = DateTime.Now, ParentID = (int)Enums.MessageTypes.All, Name = "None", Text = "Неопределенные" },
                new MessageType { ID = (int)Enums.MessageTypes.Inbox, CreateDate = DateTime.Now, ParentID = (int)Enums.MessageTypes.All, Name = "Inbox", Text = "Входящие" },

                new MessageType { ID = (int)Enums.MessageTypes.InboxNone, CreateDate = DateTime.Now, ParentID = (int)Enums.MessageTypes.Inbox, Name = "InboxNone", Text = "Неопределенные входящие" },
                new MessageType { ID = (int)Enums.MessageTypes.InboxParticipant, CreateDate = DateTime.Now, ParentID = (int)Enums.MessageTypes.Inbox, Name = "InboxParticipant", Text = "От всех участников" },
                new MessageType { ID = (int)Enums.MessageTypes.InboxSpeaker, CreateDate = DateTime.Now, ParentID = (int)Enums.MessageTypes.InboxParticipant, Name = "InboxSpeaker", Text = "От выступающих" },
                new MessageType { ID = (int)Enums.MessageTypes.InboxListener, CreateDate = DateTime.Now, ParentID = (int)Enums.MessageTypes.InboxParticipant, Name = "InboxListener", Text = "От слушателей" }
                /// ...
            });

            modelBuilder.Entity<MessageStatus>().HasData(new List<MessageStatus>
            {
                new MessageStatus { ID = (int)Enums.MessageStatuses.None, CreateDate = DateTime.Now, Name = "None", Text = "Не определен" },
                new MessageStatus { ID = (int)Enums.MessageStatuses.OtherMail, CreateDate = DateTime.Now, Name = "OtherMail", Text = "Не требует ответа" },
                new MessageStatus { ID = (int)Enums.MessageStatuses.New, CreateDate = DateTime.Now, Name = "New", Text = "Отправить ответ" },
                new MessageStatus { ID = (int)Enums.MessageStatuses.Success, CreateDate = DateTime.Now, Name = "Success", Text = "Ответ отправлен" },
                new MessageStatus { ID = (int)Enums.MessageStatuses.Error, CreateDate = DateTime.Now, Name = "Error", Text = "Ошибка. Повторить отправку" },
                new MessageStatus { ID = (int)Enums.MessageStatuses.Process, CreateDate = DateTime.Now, Name = "Process", Text = "Ответ отправляется" }
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
