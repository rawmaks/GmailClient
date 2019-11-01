using DataAccessLayer.DBContext;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        //protected readonly DbContext _dbContext;
        //protected readonly DbSet<T> _dbSet;

        //public BaseRepository(DbContext context)
        //{
        //    _dbContext = context;
        //    _dbSet = context.Set<T>();
        //}

        protected readonly DbContextOptions<MessageContext> _options;

        public BaseRepository(DbContextOptions<MessageContext> options)
        {
            _options = options;
        }


        public async Task<IEnumerable<T>> GetListAsync()
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                return await dbSet.ToListAsync();
            }
        }

        public async Task<T> GetByIDAsync(int id)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                return await dbSet.FindAsync(id);
            }
        }

        public async Task<IEnumerable<T>> FindListAsync(Func<T, bool> predicate)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                IEnumerable<T> list = await dbSet.ToListAsync();
                return list.Where(predicate);
            }
        }

        public async Task<T> FindAsync(Func<T, bool> predicate)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                IEnumerable<T> list = await dbSet.ToListAsync();
                return list.FirstOrDefault(predicate);
            }
        }

        public async Task CreateAsync(T item)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                dbSet.Add(item);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T item)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(T item)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                db.Entry(item).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                return await dbSet.AnyAsync(predicate);
            }
        }



        public bool Any(Expression<Func<T, bool>> predicate)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                return dbSet.Any(predicate);
            }
        }

        public IEnumerable<T> GetList()
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                return dbSet.ToList();
            }
        }

        public T GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindList(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public T Find(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Create(T item)
        {
            using (MessageContext db = new MessageContext(_options))
            {
                DbSet<T> dbSet = db.Set<T>();
                dbSet.Add(item);
                db.SaveChanges();
            }
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public void Delete(T item)
        {
            throw new NotImplementedException();
        }
    }
}
