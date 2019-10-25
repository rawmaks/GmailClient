using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetListAsync();
        Task<T> GetByIDAsync(int id);
        Task<IEnumerable<T>> FindAsync(Func<T, bool> predicate);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(T item);
    }
}
