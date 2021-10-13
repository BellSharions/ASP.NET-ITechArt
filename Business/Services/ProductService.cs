using Business.DTO;
using Business.Interfaces;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductByIdAsync(int id) => 
            await _productRepository.
            GetProductByIdAsync(id);


        public async Task<List<TopPlatformDto>> GetTopPlatformsAsync(int count = 3) =>
            await _productRepository.
            GetTopPlatformsAsync(count);


        public async Task<List<Product>> SearchProductByNameAsync(string term, int limit, int offset) => 
            await _productRepository.
            GetProductByNameAsync(term, limit, offset);
    }
}
