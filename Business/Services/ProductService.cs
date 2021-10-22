using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly CloudinaryOptions _options;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IOptions<CloudinaryOptions> SmtpOptionsAccessor, IMapper mapper)
        {
            _productRepository = productRepository;
            _options = SmtpOptionsAccessor.Value;
            _mapper = mapper;
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
                Platform = result.Platform.ToString(),
                Genre = result.Genre.ToString(),
                Rating = result.Rating.ToString(),
                Logo = result.Logo,
                Background = result.Background,
                Price = result.Price,
                Count = result.Count,
                DateCreated = result.DateCreated,
                TotalRating = result.TotalRating
            };
            return info;
        }

        public async Task<ServiceResult> CreateProductAsync(ProductCreationDto info)
        {
            if (info == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            var upload = new CloudinaryService(_options);
            var logoResult = await upload.UploadImage(info.Logo);
            var bgResult = await upload.UploadImage(info.Background);
            Product product = new Product()
            {
                Name = info.Name,
                Platform = info.Platform,
                Genre = info.Genre,
                Rating = info.Rating,
                Logo = logoResult,
                Background = bgResult,
                Price = info.Price,
                Count = info.Count,
                DateCreated = info.DateCreated,
                TotalRating = info.TotalRating
            };
            await _productRepository.CreateAsync(product);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ServiceResult> ChangeProductInfoAsync(ProductChangeDto info)
        {
            if (info == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            var foundProduct = await _productRepository.GetProductByIdAsync(info.Id);
            var upload = new CloudinaryService(_options);
            var logoResult = await upload.UploadImage(info.Logo);
            var bgResult = await upload.UploadImage(info.Background);
            _mapper.Map(info, foundProduct);
            foundProduct.Logo = logoResult;
            foundProduct.Background = bgResult;
            await _productRepository.UpdateItemAsync(foundProduct);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ServiceResult> DeleteProduct(int id) => 
            await _productRepository.
            DeleteProductAsync(id);

        public async Task<List<TopPlatformDto>> GetTopPlatformsAsync(int count = 3) =>
            await _productRepository.
            GetTopPlatformsAsync(count);


        public async Task<List<Product>> SearchProductByNameAsync(string term, int limit, int offset) => 
            await _productRepository.
            GetProductByNameAsync(term, limit, offset);
    }
}
