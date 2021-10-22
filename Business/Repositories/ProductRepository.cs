using Business.DTO;
using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class ProductRepository : GenericRepository<ApplicationDbContext, Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<TopPlatformDto>> GetTopPlatformsAsync(int count = 3)
        {
            var result = await _dbContext.Products
                .AsNoTracking()
                .GroupBy(u => u.Platform)
                .OrderByDescending(u => u.Count())
                .Select(u => new TopPlatformDto{ Platform = u.Key.ToString(), Count = u.Count()})
                .Take(count)
                .ToListAsync();
            return result;
        }

        public async Task<Product> GetProductByIdAsync(int id) => 
            await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

        public async Task<List<Product>> GetProductByNameAsync(string term, int limit, int offset)
        {
            var result = await _dbContext.Products.
                AsNoTracking().
                Where(u => EF.Functions.Like(u.Name, $"{term}%")).
                Skip(offset).
                Take(limit).
                ToListAsync();
            return result;
        }

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var result = await _dbContext.Products.FirstOrDefaultAsync(t => t.Id == id);
            if(result == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid id");
            result.IsDeleted = true;
            _dbContext.SaveChanges();
            return new ServiceResult(ResultType.Success, "Success");
        }
    }
}
