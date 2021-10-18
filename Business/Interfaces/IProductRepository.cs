using Business.DTO;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetProductByNameAsync(string term, int limit, int offset);
        Task<List<TopPlatformDto>> GetTopPlatformsAsync(int count);
        Task<Product> GetProductByIdAsync(int id);
        Task<ServiceResult> DeleteProductAsync(int id);
        Task<List<Product>> ListProductPageAsync(ListProductPageDto info);

    }
}
