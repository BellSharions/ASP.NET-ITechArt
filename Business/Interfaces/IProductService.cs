using Business.DTO;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<List<TopPlatformDTO>> GetTopPlatformsAsync(int count);
        Task<List<Product>> SearchProductByNameAsync(string term, int limit, int offset);
        Task<Product> GetProductByIdAsync(int id);
    }
}
