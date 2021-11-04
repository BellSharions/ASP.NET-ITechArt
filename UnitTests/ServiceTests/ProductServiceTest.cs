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
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using UnitTests.Consts;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class ProductServiceTest
    {
        public IProductRepository productRepository = A.Fake<IProductRepository>();
        public ICloudinaryService cloudinaryService = A.Fake<ICloudinaryService>();
        public IRatingRepository ratingRepository = A.Fake<IRatingRepository>();
        public IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        [Fact]
        public async Task GetTopPlatforms_ReturnResultWithListOfTopPlatformDto()
        {
            var count = 3;
            var platforms = new List<TopPlatformDto>();
            platforms.Add(Products.TopTest1);
            platforms.Add(Products.TopTest2);
            platforms.Add(Products.TopTest3);

            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            A.CallTo(() => productRepository.GetTopPlatformsAsync(count)).Returns(platforms);

            //Act
            var result = await productService.GetTopPlatformsAsync(count);

            //Assert
            Assert.Equal(platforms, result);
            Assert.Equal(platforms.Count, result.Count);

            A.CallTo(() => productRepository.GetTopPlatformsAsync(count)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetProductByIdPositive_ReturnProduct()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            var product = Products.TestProduct1;

            A.CallTo(() => productRepository.GetProductByIdAsync(product.Id)).Returns(product);

            //Act
            var result = await productService.GetProductByIdAsync(product.Id);

            //Assert
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.IsDeleted, result.IsDeleted);
            Assert.Equal(product.TotalRating, result.TotalRating);
            Assert.Equal(product.Rating, result.Rating);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Platform, result.Platform);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Logo, result.Logo);
            Assert.Equal(product.Count, result.Count);
            Assert.Equal(product.Genre, result.Genre);
            Assert.Equal(product.Background, result.Background);

            A.CallTo(() => productRepository.GetProductByIdAsync(product.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetProductInfoByIdAsyncPositive_ReturnProductInfoDto()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            var product = Products.TestProduct1;

            A.CallTo(() => productRepository.GetProductByIdAsync(product.Id)).Returns(product);

            //Act
            var result = await productService.GetProductInfoByIdAsync(product.Id);

            //Assert
            Assert.Equal(product.TotalRating, result.TotalRating);
            Assert.Equal(product.Rating.ToString(), result.Rating);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Platform.ToString(), result.Platform);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Logo, result.Logo);
            Assert.Equal(product.Count, result.Count);
            Assert.Equal(product.Genre.ToString(), result.Genre);
            Assert.Equal(product.Background, result.Background);

            A.CallTo(() => productRepository.GetProductByIdAsync(product.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetProductInfoByIdAsyncNegative_ReturnNull()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            var product = new Product();

            A.CallTo(() => productRepository.GetProductByIdAsync(product.Id)).Returns(product);

            //Act
            var result = await productService.GetProductInfoByIdAsync(product.Id);

            //Assert
            Assert.Null(result);

            A.CallTo(() => productRepository.GetProductByIdAsync(product.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ListProductAsyncPositive_ReturnListProductPageDto()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            var list = Products.TestListPage1;
            var data = new ProductListFilteredDto();
            data.Products = new List<Product>();
            data.Products.Add(Products.TestProduct1);
            data.Products.Add(Products.TestProduct2);
            data.Total = 2;
            var result = Products.TestPage1;

            A.CallTo(() => productRepository.ListProductPageAsync(list)).Returns(data);

            //Act
            var totalList = await productService.ListProductAsync(list);

            //Assert
            Assert.Equal(result.Data, totalList.Data);
            Assert.Equal(result.PageNumber, totalList.PageNumber);
            Assert.Equal(result.PageSize, totalList.PageSize);
            Assert.Equal(result.ProductAmount, totalList.ProductAmount);

            A.CallTo(() => productRepository.ListProductPageAsync(list)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteProductAsyncPositive_ReturnServiceResultWithOK()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int id = 1;
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => productRepository.DeleteProductAsync(id)).Returns(true);

            //Act
            var result = await productService.DeleteProductAsync(id);

            //Assert
            Assert.Equal(ResultType.Success, result.Type);

            A.CallTo(() => productRepository.DeleteProductAsync(id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteProductAsyncNegative_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int id = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid Id");

            A.CallTo(() => productRepository.DeleteProductAsync(id)).Returns(false);

            //Act
            var result = await productService.DeleteProductAsync(id);

            //Assert
            Assert.Equal(ResultType.BadRequest, result.Type);

            A.CallTo(() => productRepository.DeleteProductAsync(id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task CreateProductAsyncPositive_ReturnServiceResultWithOk()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => productRepository.CreateAsync(Products.TestProduct1)).Returns(true);

            //Act
            var result = await productService.CreateProductAsync(Products.TestCreation1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productRepository.CreateAsync(Products.TestProduct1)).MustHaveHappenedOnceOrLess();
        }
        [Fact]
        public async Task CreateProductAsyncNegative_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);
            var nullProduct = new Product();
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid Id");

            A.CallTo(() => productRepository.CreateAsync(Products.TestProduct1)).Returns(false);

            //Act
            var result = await productService.CreateProductAsync(new ProductCreationDto());

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productRepository.CreateAsync(Products.TestProduct1)).MustNotHaveHappened();
        }
        [Fact]
        public async Task DeleteRatingAsyncPositive_ReturnServiceResultWithOK()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int productId = 1;
            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => ratingRepository.DeleteAsync(A<Expression<Func<ProductRating, bool>>>.Ignored)).Returns(true);

            //Act
            var result = await productService.DeleteRatingAsync(userId, productId);

            //Assert
            Assert.Equal(ResultType.Success, result.Type);

            A.CallTo(() => ratingRepository.DeleteAsync(A<Expression<Func<ProductRating, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteRatingAsyncNegative_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int productId = 1;
            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Success");

            A.CallTo(() => ratingRepository.DeleteAsync(A<Expression<Func<ProductRating, bool>>>.Ignored)).Returns(false);

            //Act
            var result = await productService.DeleteRatingAsync(userId, productId);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => ratingRepository.DeleteAsync(A<Expression<Func<ProductRating, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task AddRatingAsyncPositive_ReturnServiceResultWithOk()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).Returns(true);

            //Act
            var result = await productService.AddRatingAsync(userId, Products.TestRatingCreation1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task AddRatingAsyncNegative_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Success");
            var nullRating = new ProductRating();

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).Returns(false);

            //Act
            var result = await productService.AddRatingAsync(userId, Products.TestRatingCreation1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task AddRatingAsyncNegative_RatingLessThanZero_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Success");
            var nullRating = new ProductRating();

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).Returns(false);

            //Act
            var result = await productService.AddRatingAsync(userId, Products.TestRatingCreationOutOfBounds1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task AddRatingAsyncNegative_RatingMoreThanHundred_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Success");
            var nullRating = new ProductRating();

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).Returns(false);

            //Act
            var result = await productService.AddRatingAsync(userId, Products.TestRatingCreationOutOfBounds2);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task SearchProductByNameAsyncPositive_ReturnSearchResult()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            var searchList = Products.TestSearchList1;


            A.CallTo(() => productRepository.GetProductByNameAsync("Genshin Impact", 1, 0)).Returns(Products.TestList1);

            //Act
            var result = await productService.SearchProductByNameAsync("Genshin Impact", 1, 0);

            //Assert
            Assert.Equal(searchList.Count, result.Count);
            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(searchList[i].Name, result[i].Name);
                Assert.Equal(searchList[i].Platform, result[i].Platform);
                Assert.Equal(searchList[i].Price, result[i].Price);
                Assert.Equal(searchList[i].Rating, result[i].Rating);
                Assert.Equal(searchList[i].TotalRating, result[i].TotalRating);
                Assert.Equal(searchList[i].Background, result[i].Background);
                Assert.Equal(searchList[i].Count, result[i].Count);
                Assert.Equal(searchList[i].DateCreated, result[i].DateCreated);
                Assert.Equal(searchList[i].Genre, result[i].Genre);
                Assert.Equal(searchList[i].Logo, result[i].Logo);
            }

            A.CallTo(() => productRepository.GetProductByNameAsync("Genshin Impact", 1, 0)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncPositive_ReturnServiceResultWithOk()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var nullRating = new ProductRating();
            var product = Products.TestProduct1;

            A.CallTo(() => productRepository.GetProductByIdAsync(product.Id)).Returns(product);
            A.CallTo(() => cloudinaryService.DeleteImage(Products.TestProduct3.Logo)).Returns(serviceResult);
            A.CallTo(() => productRepository.UpdateItemAsync(Products.TestProduct1)).Returns(true);

            //Act
            var result = await productService.ChangeProductInfoAsync(userId, Products.TestChange1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncNegative_UpdateHaulted_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var nullRating = new ProductRating();
            var product = Products.TestProduct1;

            A.CallTo(() => productRepository.GetProductByIdAsync(product.Id)).Returns(product);
            A.CallTo(() => productRepository.UpdateItemAsync(Products.TestProduct1)).Returns(false);

            //Act
            var result = await productService.ChangeProductInfoAsync(userId, Products.TestChange1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncNegative_InfoIsNull_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var nullRating = new ProductRating();
            var nullProduct = new Product();

            A.CallTo(() => productRepository.GetProductByIdAsync(nullProduct.Id)).Returns(nullProduct);
            A.CallTo(() => productRepository.UpdateItemAsync(Products.TestProduct1)).Returns(false);

            //Act
            var result = await productService.ChangeProductInfoAsync(userId, null);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productRepository.UpdateItemAsync(A<Product>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncNegative_InvalidId_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var nullProduct = new Product();

            A.CallTo(() => productRepository.GetProductByIdAsync(nullProduct.Id)).Returns(nullProduct);
            A.CallTo(() => productRepository.UpdateItemAsync(Products.TestProduct1)).Returns(false);

            //Act
            var result = await productService.ChangeProductInfoAsync(userId, Products.TestChange1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productRepository.UpdateItemAsync(A<Product>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncNegative_NullLogo_ReturnServiceResultWithBadRequest()
        {
            var productService = new ProductService(productRepository, ratingRepository, cloudinaryService, mapper);

            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var nullProduct = new Product();

            A.CallTo(() => productRepository.GetProductByIdAsync(Products.TestProduct3.Id)).Returns(Products.TestProduct3);
            A.CallTo(() => productRepository.UpdateItemAsync(Products.TestProduct3)).Returns(false);
            A.CallTo(() => cloudinaryService.DeleteImage(Products.TestProduct3.Logo)).Returns(serviceResult);
            //Act
            var result = await productService.ChangeProductInfoAsync(2, Products.TestChange1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productRepository.UpdateItemAsync(A<Product>.Ignored)).MustNotHaveHappened();
        }
    }
}
