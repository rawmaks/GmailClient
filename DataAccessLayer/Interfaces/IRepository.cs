using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T>
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetListAsync();
        Task<T> GetByIDAsync(int id);
        Task<IEnumerable<T>> FindListAsync(Func<T, bool> predicate);
        Task<T> FindAsync(Func<T, bool> predicate);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(T item);


        bool Any(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetList();
        T GetByID(int id);
        IEnumerable<T> FindList(Func<T, bool> predicate);
        T Find(Func<T, bool> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
