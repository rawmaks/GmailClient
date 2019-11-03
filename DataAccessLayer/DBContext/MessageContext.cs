using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
                new MessageType { ID = 1, CreateDate = DateTime.Now, ParentID = default, Name = "All", Text = "Все" },
                new MessageType { ID = 2, CreateDate = DateTime.Now, ParentID = 1, Name = "None", Text = "Неопределенные" },
                new MessageType { ID = 3, CreateDate = DateTime.Now, ParentID = 1, Name = "Other", Text = "Другие" },
                new MessageType { ID = 4, CreateDate = DateTime.Now, ParentID = 1, Name = "Participant", Text = "От участников" },
                new MessageType { ID = 5, CreateDate = DateTime.Now, ParentID = 4, Name = "Listener", Text = "От слушателей" }
                /// ...
            });

            modelBuilder.Entity<MessageStatus>().HasData(new List<MessageStatus>
            {
                new MessageStatus { ID = 1, CreateDate = DateTime.Now, Name = "New" },
                new MessageStatus { ID = 2, CreateDate = DateTime.Now, Name = "Success" },
                new MessageStatus { ID = 3, CreateDate = DateTime.Now, Name = "Error" },
                new MessageStatus { ID = 4, CreateDate = DateTime.Now, Name = "Process" }
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
