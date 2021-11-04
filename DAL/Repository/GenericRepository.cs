using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public abstract class GenericRepository<TContext, T> : IRepository<T>
        where TContext : ApplicationDbContext
        where T : class
    {
        protected readonly TContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(TContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<bool> CreateAsync(T item)
        {
            try
            {
                await _dbSet.AddAsync(item);

                if (await _dbContext.SaveChangesAsync() == 0)
                    return false;

                _dbContext.Entry(item).State = EntityState.Detached;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Could not create item in database. Error: {e.Message}");
            }

        }

        public async Task<List<T>> AddRangeAsync(IEnumerable<T> items)
        {
            var entitiesList = items.ToList();

            try
            {
                await _dbSet.AddRangeAsync(entitiesList);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Could not create items in database. Error: {e.Message}");
            }

            return entitiesList;
        }
        public async Task<bool> UpdateItemAsync(T item)
        {
            try
            {
                _dbSet.Update(item);

                if (await _dbContext.SaveChangesAsync() == 0)
                    return false;

                _dbContext.Entry(item).State = EntityState.Detached;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Unable to update item. Error: {e.Message}");
            }
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                _dbSet.RemoveRange(_dbSet.Where(expression));

                if (await _dbContext.SaveChangesAsync() == 0)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Unable to remove item or items. Error: {e.Message}");
            }
        }
    }
}
