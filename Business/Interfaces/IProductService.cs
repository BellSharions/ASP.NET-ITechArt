using Business.DTO;
using DAL.Entities;
using DAL.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<List<TopPlatformDto>> GetTopPlatformsAsync(int count);
        Task<List<Product>> SearchProductByNameAsync(string term, int limit, int offset);
        Task<Product> GetProductByIdAsync(int id);
        Task<ProductInfoDto> GetProductInfoByIdAsync(int id);
        Task<ServiceResult> CreateProductAsync(ProductCreationDto info);
        Task<ServiceResult> DeleteProduct(int id);
        Task<ServiceResult> ChangeProductInfoAsync(ProductChangeDto info);
    }
}
