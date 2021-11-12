using AutoMapper;
using Business;
using Business.DTO;
using Business.Enums;
using Business.Interfaces;
using Business.Services;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTests.Consts
{
    public class ProductServiceFixtures
    {
        public readonly IFormFile TestFormFile = new FormFile(
                new MemoryStream(Encoding.UTF8.GetBytes("This is a test")),
                0,
                Encoding.UTF8.GetBytes("This is a test").Length,
                "Data",
                "Picture.png"
            );

        public readonly string TestLink = @"";
        public Product CorrectProduct1 { get; set; }
        public Product CorrectProduct2 { get; set; }
        public Product ProductWithNull { get; set; }
        public ProductInfoDto CorrectProductInfo { get; set; }
        public ListProductPageDto CorrectListPage { get; set; }
        public TopPlatformDto TopPlatformInfo1 { get; set; }
        public TopPlatformDto TopPlatformInfo2 { get; set; }
        public TopPlatformDto TopPlatformInfo3 { get; set; }
        public ProductPageDto CorrectProductPage { get; set; }
        public ProductCreationDto CorrectProductCreation { get; set; }
        public RatingCreationDto CorrectRatingCreation { get; set; }
        public RatingCreationDto TestRatingCreationOutOfBoundsLessThanZero { get; set; }
        public RatingCreationDto TestRatingCreationOutOfBoundsMoreThanHundred { get; set; }
        public List<ProductInfoDto> CorrectSearchList { get; set; }
        public List<Product> CorrectListOfProducts { get; set; }
        public ProductChangeDto CorrectChangeInformation { get; set; }
        public IProductRepository productRepository { get; set; }
        public ICloudinaryService cloudinaryService { get; set; }
        public IRatingRepository ratingRepository { get; set; }
        public IMapper mapper { get; set; }
        public ProductService productService { get; set; }
        public ServiceResult serviceResultOk { get; set; }
        public ServiceResult serviceResultBadRequest { get; set; }
        public ServiceResult serviceResultCreated { get; set; }

        public ProductServiceFixtures()
        {
            CorrectProduct1 = new()
            {
                Id = 1,
                Name = "Genshin Impact",
                Platform = AvailablePlatforms.PC,
                TotalRating = 87,
                Genre = AvailableGenres.Adventure,
                Rating = AgeRating.PEGI16,
                Logo = "",
                Background = "",
                Price = 1,
                Count = 150
            };
            CorrectProduct2 = new()
            {
                Id = 2,
                Name = "Ultrakill",
                Platform = AvailablePlatforms.PC,
                TotalRating = 99,
                Genre = AvailableGenres.FPS,
                Rating = AgeRating.PEGI18,
                Logo = "",
                Background = "",
                Price = 99,
                Count = 99
            };
            ProductWithNull = new()
            {
                Id = 3,
                Name = "Ultrakill",
                Platform = AvailablePlatforms.PC,
                TotalRating = 99,
                Genre = AvailableGenres.FPS,
                Rating = AgeRating.PEGI18,
                Logo = null,
                Background = null,
                Price = 99,
                Count = 99
            };
            CorrectProductInfo = new()
            {
                Name = "Genshin Impact",
                Platform = "PC",
                TotalRating = 87,
                Genre = "Adventure",
                Rating = "PEGI16",
                Logo = "",
                Background = "",
                Price = 1,
                Count = 150
            };
            CorrectListPage = new()
            {
                PageNumber = 1,
                PageSize = 10,
                PriceSort = Sorting.Asc,
                RatingSort = Sorting.Ignore,
                Genre = AvailableGenres.Sandbox,
                AgeRating = AgeRating.PEGI16
            };
            TopPlatformInfo1 = new()
            {
                Platform = "PC",
                Count = 5
            };
            TopPlatformInfo2 = new()
            {
                Platform = "XBOX",
                Count = 3
            };
            TopPlatformInfo3 = new()
            {
                Platform = "PlayStation",
                Count = 1
            };
            CorrectProductPage = new()
            {
                PageNumber = 1,
                PageSize = 10,
                Data = new List<Product> { CorrectProduct1, CorrectProduct2 },
                ProductAmount = 2
            };
            CorrectProductCreation = new()
            {
                Name = "Ultrakill",
                Platform = AvailablePlatforms.PC,
                TotalRating = 99,
                Genre = AvailableGenres.FPS,
                Rating = AgeRating.PEGI18,
                Logo = TestFormFile,
                Background = TestFormFile,
                Price = 99,
                Count = 99
            };
            CorrectRatingCreation = new()
            {
                ProductId = 1,
                Rating = 90
            };
            TestRatingCreationOutOfBoundsLessThanZero = new()
            {
                ProductId = 1,
                Rating = -5
            };
            TestRatingCreationOutOfBoundsMoreThanHundred = new()
            {
                ProductId = 1,
                Rating = 110
            };
            CorrectSearchList = new()
            {
                CorrectProductInfo
            };
            CorrectListOfProducts = new()
            {
                CorrectProduct1
            };
            CorrectChangeInformation = new()
            {
                Name = "Ultrakill",
                Platform = AvailablePlatforms.PC,
                TotalRating = 99,
                Genre = AvailableGenres.FPS,
                Rating = AgeRating.PEGI18,
                Logo = TestFormFile,
                Background = TestFormFile,
                Price = 99,
                Count = 99
            };

            productRepository = A.Fake<IProductRepository>();
            cloudinaryService = A.Fake<ICloudinaryService>();
            ratingRepository = A.Fake<IRatingRepository>();
            mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
            productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);
            serviceResultOk = new ServiceResult(ResultType.Success, "Success");
            serviceResultBadRequest = new ServiceResult(ResultType.BadRequest, "Invalid Information");
            serviceResultCreated = new ServiceResult(ResultType.Created, "Created");
        }
    }
}
