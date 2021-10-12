using Business.DTO;
using Business.Enums;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetProductByNameAsync(string term, int limit, int offset);
        Task<List<TopPlatformDTO>> GetTopPlatformsAsync(int count);
        Task<Product> GetProductByIdAsync(int id);

    }
}
