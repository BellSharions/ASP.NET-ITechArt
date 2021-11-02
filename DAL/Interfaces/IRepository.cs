using DAL.Entities.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);
    }
}
