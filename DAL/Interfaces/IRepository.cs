using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T item);
        Task<int> CountAsync(Expression<Func<T, bool>> expression);
        Task<T> UpdateItemAsync(T item);
        Task DeleteAsync(Expression<Func<T, bool>> expression);
    }
}
