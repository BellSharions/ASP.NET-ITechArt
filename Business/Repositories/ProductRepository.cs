using Business.DTO;
using Business.Enums;
using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class ProductRepository : GenericRepository<ApplicationDbContext, Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<List<TopPlatformDTO>> GetTopPlatformsAsync(int count = 3) => 
            (await _dbContext.Products.GroupBy(u => u.Platform).Select(u => new TopPlatformDTO(u.Key.ToString(), u.Count())).ToListAsync()).OrderByDescending(u => u.Count).Take(count).ToList();

        public async Task<Product> GetProductByIdAsync(int id) => 
            await _dbContext.Products.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<List<Product>> GetProductByNameAsync(string term, int limit, int offset) =>
            await _dbContext.Products.AsNoTracking().Where(t => EF.Functions.Like(t.Name, $"{term}%")).Skip(offset).Take(limit).ToListAsync();
    }
}
