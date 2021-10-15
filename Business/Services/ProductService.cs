using Business.DTO;
using Business.Interfaces;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly ICloudinaryService _cloudinaryService;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductByIdAsync(int id) => 
            await _productRepository.
            GetProductByIdAsync(id);

        public async Task<ProductInfoDto> GetProductInfoByIdAsync(int id)
        {
            var result = await _productRepository.GetProductByIdAsync(id);
            var info = new ProductInfoDto
            {
                Name = result.Name,
                Platform = result.Platform,
                Genre = result.Genre,
                Rating = result.Rating,
                Logo = result.Logo,
                Background = result.Background,
                Price = result.Price,
                Count = result.Count,
                DateCreated = result.DateCreated,
                TotalRating = result.TotalRating
            };
            return info;
        }

        public async Task<ServiceResult> CreateProductAsync(ProductInfoDto info)
        {
            if (info == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            Product product = new Product()
            {
                Name = info.Name,
                Platform = info.Platform,
                Genre = info.Genre,
                Rating = info.Rating,
                Logo = info.Logo,
                Background = info.Background,
                Price = info.Price,
                Count = info.Count,
                DateCreated = info.DateCreated,
                TotalRating = info.TotalRating
            };
            await _productRepository.CreateAsync(product);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ServiceResult> ChangeProductInfoAsync(ProductInfoDto info)
        {
            if (info == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            Product product = new Product()
            {
                Name = info.Name,
                Platform = info.Platform,
                Genre = info.Genre,
                Rating = info.Rating,
                Logo = info.Logo,
                Background = info.Background,
                Price = info.Price,
                Count = info.Count,
                DateCreated = info.DateCreated,
                TotalRating = info.TotalRating
            };
            await _productRepository.CreateAsync(product);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<List<TopPlatformDto>> GetTopPlatformsAsync(int count = 3) =>
            await _productRepository.
            GetTopPlatformsAsync(count);


        public async Task<List<Product>> SearchProductByNameAsync(string term, int limit, int offset) => 
            await _productRepository.
            GetProductByNameAsync(term, limit, offset);
    }
}
