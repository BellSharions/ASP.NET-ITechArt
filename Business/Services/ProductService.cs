using AutoMapper;
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
        private readonly IRatingRepository _ratingRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IRatingRepository ratingRepository, ICloudinaryService clodinaryService, IMapper mapper)
        {
            _productRepository = productRepository;
            _ratingRepository = ratingRepository;
            _cloudinaryService = clodinaryService;
            _mapper = mapper;
        }

        public async Task<Product> GetProductByIdAsync(int id) => 
            await _productRepository.
            GetProductByIdAsync(id);

        public async Task<ProductInfoDto> GetProductInfoByIdAsync(int id)
        {
            var result = await _productRepository.GetProductByIdAsync(id);
            if(result.Id == 0)
                return null;
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
            if (info.Name == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            var logoResult = await _cloudinaryService.UploadImage(info.Logo.FileName, info.Logo.OpenReadStream());
            var bgResult = await _cloudinaryService.UploadImage(info.Background.FileName, info.Background.OpenReadStream());
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

        public async Task<ServiceResult> ChangeProductInfoAsync(int id, ProductChangeDto info)
        {
            if (info == null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");

            var foundProduct = await _productRepository.GetProductByIdAsync(id);
            if (foundProduct == null)
                return new ServiceResult(ResultType.BadRequest, "No product was found");

            var deletionResult = _cloudinaryService.DeleteImage(foundProduct.Logo);
            if (deletionResult == null)
                return new ServiceResult(ResultType.BadRequest, "Deletion was haulted");

            var logoResult = await _cloudinaryService.UploadImage(info.Logo.FileName, info.Logo.OpenReadStream());
            var bgResult = await _cloudinaryService.UploadImage(info.Background.FileName, info.Background.OpenReadStream());
            _mapper.Map(info, foundProduct);
            foundProduct.Logo = logoResult;
            foundProduct.Background = bgResult;
            if(await _productRepository.UpdateItemAsync(foundProduct))
                return new ServiceResult(ResultType.Success, "Success");
            return new ServiceResult(ResultType.BadRequest, "Update was haulted");
        }

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var result = await _productRepository.DeleteProductAsync(id);
            if (!result)
                return new ServiceResult(ResultType.BadRequest, "Invalid id");
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<List<TopPlatformDto>> GetTopPlatformsAsync(int count = 3) =>
            await _productRepository.
            GetTopPlatformsAsync(count);


        public async Task<List<ProductInfoDto>> SearchProductByNameAsync(string term, int limit, int offset)
        {
            var queryResult = await _productRepository.GetProductByNameAsync(term, limit, offset);
            var result = new List<ProductInfoDto>();
            _mapper.Map(queryResult, result);
            return result;
        }

        public async Task<ServiceResult> AddRatingAsync(int userId, RatingCreationDto info)
        {
            if(info.Rating < 0 || info.Rating > 100)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            var rating = new ProductRating()
            {
                UserId = userId,
                ProductId = info.ProductId,
                Rating = info.Rating
            };
            var result = await _ratingRepository.CreateAsync(rating);
            if (result.Rating != info.Rating)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            await _productRepository.RecalculateRating(info.ProductId);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ServiceResult> DeleteRatingAsync(int userId, int productId)
        {
            var result = await _ratingRepository.DeleteAsync(u => u.UserId == userId && u.ProductId == productId);
            if (result == false)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            await _productRepository.RecalculateRating(productId);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ProductPageDto> ListProductAsync(ListProductPageDto info)
        {
            var list = await _productRepository.ListProductPageAsync(info);
            var result = new ProductPageDto
            {
                Data = list.Products,
                PageNumber = info.PageNumber,
                PageSize = info.PageSize,
                ProductAmount = list.Total
            };
            return result;
        }
    }
}
