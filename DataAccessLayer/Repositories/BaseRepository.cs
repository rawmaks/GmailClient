using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }


        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIDAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate)
        {
            IEnumerable<T> list = await _dbSet.ToListAsync();
            return list.Where(predicate);
        }

        public async Task CreateAsync(T item)
        {
            _dbSet.Add(item);
        }

        public async Task UpdateAsync(T item)
        {
            // #1
            //_dbSet.Update(item); // InsertOrUpdate

            // #2
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public async Task DeleteAsync(T item)
        {
            // #1
            //_dbSet.Remove(item);

            // #2
            _dbContext.Entry(item).State = EntityState.Deleted;
        }

    }
}
