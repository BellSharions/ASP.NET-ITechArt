using Business.DTO;
using Business.Enums;
using DAL.Entities;
using DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Consts
{
    public static class Products
    {
        public static readonly IFormFile TestFormFile = new FormFile(
                new MemoryStream(Encoding.UTF8.GetBytes("This is a test")),
                0,
                Encoding.UTF8.GetBytes("This is a test").Length,
                "Data",
                "Picture.png"
            );

        public static readonly string TestLink = @"";

        public static Product TestProduct1 = new()
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

        public static Product TestProduct2 = new()
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
        public static Product TestProduct3 = new()
        {
            Id = 2,
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

        public static ProductInfoDto TestProductInfo1 = new()
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
        public static ListProductPageDto TestListPage1 = new()
        {
            PageNumber = 1,
            PageSize = 10,
            PriceSort = Sorting.Asc,
            RatingSort = Sorting.Ignore,
            Genre = AvailableGenres.Sandbox,
            AgeRating = AgeRating.PEGI16
        };

        public static TopPlatformDto TopTest1 = new()
        {
            Platform = "PC",
            Count = 5
        };
        public static TopPlatformDto TopTest2 = new()
        {
            Platform = "XBOX",
            Count = 3
        };
        public static TopPlatformDto TopTest3 = new()
        {
            Platform = "PlayStation",
            Count = 1
        };
        public static ProductPageDto TestPage1 = new()
        {
            PageNumber = 1,
            PageSize = 10,
            Data = new List<Product> { TestProduct1, TestProduct2 },
            ProductAmount = 2
        };
        public static ProductCreationDto TestCreation1 = new()
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
        public static ProductRating TestRating1 = new()
        {
            UserId = 1,
            ProductId = 1,
            Rating = 90
        };
        public static RatingCreationDto TestRatingCreation1 = new()
        {
            ProductId = 1,
            Rating = 90
        };
        public static RatingCreationDto TestRatingCreationOutOfBounds1 = new()
        {
            ProductId = 1,
            Rating = -5
        };
        public static RatingCreationDto TestRatingCreationOutOfBounds2 = new()
        {
            ProductId = 1,
            Rating = 110
        };
        public static List<ProductInfoDto> TestSearchList1 = new()
        {
            TestProductInfo1
        };
        public static List<Product> TestList1 = new()
        {
            TestProduct1
        };
        public static ProductChangeDto TestChange1 = new()
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
    }
}
