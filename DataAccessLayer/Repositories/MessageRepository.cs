﻿using DataAccessLayer.DBContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MessageRepository<T> : BaseRepository<T>, IMessageRepository<T> where T : class
    {
        //public MessageRepository(MessageContext context) : base(context)
        //{
        //}

        public MessageRepository(DbContextOptions<MessageContext> options) : base(options)
        {

        }
    }
}
