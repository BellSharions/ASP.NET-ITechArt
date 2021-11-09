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

        public Product TestProduct1 { get; set; }
        public Product TestProduct2 { get; set; }
        public Product TestProduct3 { get; set; }
        public List<Product> TestProductList1 { get; set; }
        public ProductInfoDto TestProductInfo1 { get; set; }
        public ProductInfoDto TestProductInfo2 { get; set; }
        public ListProductPageDto TestListPage1 { get; set; }
        public TopPlatformDto TopTest1 { get; set; }
        public TopPlatformDto TopTest2 { get; set; }
        public TopPlatformDto TopTest3 { get; set; }
        public ProductPageDto TestPage1 { get; set; }
        public ProductCreationDto TestCreation1 { get; set; }
        public ProductRating TestRating1 { get; set; }
        public RatingCreationDto TestRatingCreation1 { get; set; }
        public RatingCreationDto TestRatingCreationOutOfBoundsLessThanZero { get; set; }
        public RatingCreationDto TestRatingCreationOutOfBoundsMoreThanHundred { get; set; }
        public List<ProductInfoDto> TestSearchList1 { get; set; }
        public List<Product> TestList1 { get; set; }
        public ProductChangeDto TestChange1 { get; set; }
        public IProductRepository productRepository { get; set; }
        public ICloudinaryService cloudinaryService { get; set; }
        public IRatingRepository ratingRepository { get; set; }
        public IMapper mapper { get; set; }
        public ProductService productService { get; set; }
        public ServiceResult serviceResultOk { get; set; }
        public ServiceResult serviceResultBadRequest { get; set; }
        public ServiceResult serviceResultCreated { get; set; }

        public ProductServiceFixtures() {
        TestProduct1 = new()
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
        TestProduct2 = new()
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
        TestProduct3 = new()
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
        TestProductList1 = new()
        {
            TestProduct1,
            TestProduct2,
            TestProduct3
        };
        TestProductInfo1 = new()
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
        TestProductInfo2 = new()
        {
            Name = "Ultrakill",
            Platform = "PC",
            TotalRating = 99,
            Genre = "FPS",
            Rating = "PEGI18",
            Logo = "",
            Background = "",
            Price = 99,
            Count = 99
        };
        TestListPage1 = new()
        {
            PageNumber = 1,
            PageSize = 10,
            PriceSort = Sorting.Asc,
            RatingSort = Sorting.Ignore,
            Genre = AvailableGenres.Sandbox,
            AgeRating = AgeRating.PEGI16
        };
        TopTest1 = new()
        {
            Platform = "PC",
            Count = 5
        };
        TopTest2 = new()
        {
            Platform = "XBOX",
            Count = 3
        };
        TopTest3 = new()
        {
            Platform = "PlayStation",
            Count = 1
        };
        TestPage1 = new()
        {
            PageNumber = 1,
            PageSize = 10,
            Data = new List<Product> { TestProduct1, TestProduct2 },
            ProductAmount = 2
        };
        TestCreation1 = new()
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
        TestRating1 = new()
        {
            UserId = 1,
            ProductId = 1,
            Rating = 90
        };
        TestRatingCreation1 = new()
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
        TestSearchList1 = new()
        {
            TestProductInfo1
        };
        TestList1 = new()
        {
            TestProduct1
        };
        TestChange1 = new()
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
