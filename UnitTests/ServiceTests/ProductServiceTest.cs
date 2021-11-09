using Business.DTO;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitTests.Consts;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class ProductServiceTest : IClassFixture<ProductServiceFixtures>
    {
        ProductServiceFixtures productFixture;
        public ProductServiceTest(ProductServiceFixtures fixture)
        {
            this.productFixture = fixture;
            Fake.ClearRecordedCalls(productFixture.cloudinaryService);
            Fake.ClearRecordedCalls(productFixture.productRepository);
            Fake.ClearRecordedCalls(productFixture.ratingRepository);
        }
        [Fact]
        public async Task GetTopPlatforms_ReturnResultWithListOfTopPlatformDto()
        {
            var count = 3;
            var platforms = new List<TopPlatformDto>();
            platforms.Add(productFixture.TopTest1);
            platforms.Add(productFixture.TopTest2);
            platforms.Add(productFixture.TopTest3);


            A.CallTo(() => productFixture.productRepository.GetTopPlatformsAsync(count)).Returns(platforms);

            //Act
            var result = await productFixture.productService.GetTopPlatformsAsync(count);

            //Assert
            Assert.Equal(platforms, result);
            Assert.Equal(platforms.Count, result.Count);

            A.CallTo(() => productFixture.productRepository.GetTopPlatformsAsync(count)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetProductByIdPositive_ReturnProduct()
        {

            var product = productFixture.TestProduct1;

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(product.Id)).Returns(product);

            //Act
            var result = await productFixture.productService.GetProductByIdAsync(product.Id);

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

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(product.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetProductInfoByIdAsyncPositive_ReturnProductInfoDto()
        {

            var product = productFixture.TestProduct1;

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(product.Id)).Returns(product);

            //Act
            var result = await productFixture.productService.GetProductInfoByIdAsync(product.Id);

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

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(product.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetProductInfoByIdAsyncNegative_ReturnNull()
        {

            Product product = null;

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(1)).Returns(product);

            //Act
            var result = await productFixture.productService.GetProductInfoByIdAsync(1);

            //Assert
            Assert.Null(result);

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ListProductAsyncPositive_ReturnListProductPageDto()
        {

            var list = productFixture.TestListPage1;
            var data = new ProductListFilteredDto();
            data.Products = new List<Product>();
            data.Products.Add(productFixture.TestProduct1);
            data.Products.Add(productFixture.TestProduct2);
            data.Total = 2;
            var result = productFixture.TestPage1;

            A.CallTo(() => productFixture.productRepository.ListProductPageAsync(list)).Returns(data);

            //Act
            var totalList = await productFixture.productService.ListProductAsync(list);

            //Assert
            Assert.Equal(result.Data, totalList.Data);
            Assert.Equal(result.PageNumber, totalList.PageNumber);
            Assert.Equal(result.PageSize, totalList.PageSize);
            Assert.Equal(result.ProductAmount, totalList.ProductAmount);

            A.CallTo(() => productFixture.productRepository.ListProductPageAsync(list)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteProductAsyncPositive_ReturnServiceResultWithOK()
        {

            int id = 1;
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => productFixture.productRepository.DeleteProductAsync(id)).Returns(true);

            //Act
            var result = await productFixture.productService.DeleteProductAsync(id);

            //Assert
            Assert.Equal(ResultType.Success, result.Type);

            A.CallTo(() => productFixture.productRepository.DeleteProductAsync(id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteProductAsyncNegative_ReturnServiceResultWithBadRequest()
        {

            int id = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid Id");

            A.CallTo(() => productFixture.productRepository.DeleteProductAsync(id)).Returns(false);

            //Act
            var result = await productFixture.productService.DeleteProductAsync(id);

            //Assert
            Assert.Equal(ResultType.BadRequest, result.Type);

            A.CallTo(() => productFixture.productRepository.DeleteProductAsync(id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task CreateProductAsyncPositive_ReturnServiceResultWithOk()
        {

            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => productFixture.productRepository.CreateAsync(productFixture.TestProduct1)).Returns(true);

            //Act
            var result = await productFixture.productService.CreateProductAsync(productFixture.TestCreation1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.productRepository.CreateAsync(productFixture.TestProduct1)).MustHaveHappenedOnceOrLess();
        }
        [Fact]
        public async Task CreateProductAsyncNegative_ReturnServiceResultWithBadRequest()
        {
            var nullProduct = new Product();
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid Id");

            A.CallTo(() => productFixture.productRepository.CreateAsync(productFixture.TestProduct1)).Returns(false);

            //Act
            var result = await productFixture.productService.CreateProductAsync(new ProductCreationDto());

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.productRepository.CreateAsync(productFixture.TestProduct1)).MustNotHaveHappened();
        }
        [Fact]
        public async Task DeleteRatingAsyncPositive_ReturnServiceResultWithOK()
        {

            int productId = 1;
            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => productFixture.ratingRepository.DeleteAsync(A<Expression<Func<ProductRating, bool>>>.Ignored)).Returns(true);

            //Act
            var result = await productFixture.productService.DeleteRatingAsync(userId, productId);

            //Assert
            Assert.Equal(ResultType.Success, result.Type);

            A.CallTo(() => productFixture.ratingRepository.DeleteAsync(A<Expression<Func<ProductRating, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task DeleteRatingAsyncNegative_ReturnServiceResultWithBadRequest()
        {

            int productId = 1;
            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Success");

            A.CallTo(() => productFixture.ratingRepository.DeleteAsync(A<Expression<Func<ProductRating, bool>>>.Ignored)).Returns(false);

            //Act
            var result = await productFixture.productService.DeleteRatingAsync(userId, productId);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.ratingRepository.DeleteAsync(A<Expression<Func<ProductRating, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task AddRatingAsyncPositive_ReturnServiceResultWithOk()
        {

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.Success, "Success");

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).Returns(true);

            //Act
            var result = await productFixture.productService.AddRatingAsync(userId, productFixture.TestRatingCreation1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task AddRatingAsyncNegative_ReturnServiceResultWithBadRequest()
        {

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Success");
            var nullRating = new ProductRating();

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).Returns(false);

            //Act
            var result = await productFixture.productService.AddRatingAsync(userId, productFixture.TestRatingCreation1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task AddRatingAsyncNegative_RatingLessThanZero_ReturnServiceResultWithBadRequest()
        {

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Success");
            var nullRating = new ProductRating();

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).Returns(false);

            //Act
            var result = await productFixture.productService.AddRatingAsync(userId, productFixture.TestRatingCreationOutOfBoundsLessThanZero);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task AddRatingAsyncNegative_RatingMoreThanHundred_ReturnServiceResultWithBadRequest()
        {

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Success");
            var nullRating = new ProductRating();

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).Returns(false);

            //Act
            var result = await productFixture.productService.AddRatingAsync(userId, productFixture.TestRatingCreationOutOfBoundsMoreThanHundred);
            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task SearchProductByNameAsyncPositive_ReturnSearchResult()
        {

            var searchList = productFixture.TestSearchList1;


            A.CallTo(() => productFixture.productRepository.GetProductByNameAsync("Genshin Impact", 1, 0)).Returns(productFixture.TestList1);

            //Act
            var result = await productFixture.productService.SearchProductByNameAsync("Genshin Impact", 1, 0);

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

            A.CallTo(() => productFixture.productRepository.GetProductByNameAsync("Genshin Impact", 1, 0)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncPositive_ReturnServiceResultWithOk()
        {

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var nullRating = new ProductRating();
            var product = productFixture.TestProduct1;

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(product.Id)).Returns(product);
            A.CallTo(() => productFixture.cloudinaryService.DeleteImage(productFixture.TestProduct3.Logo)).Returns(serviceResult);
            A.CallTo(() => productFixture.productRepository.UpdateItemAsync(productFixture.TestProduct1)).Returns(true);

            //Act
            var result = await productFixture.productService.ChangeProductInfoAsync(userId, productFixture.TestChange1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncNegative_UpdateHaulted_ReturnServiceResultWithBadRequest()
        {

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var nullRating = new ProductRating();
            var product = productFixture.TestProduct1;

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(product.Id)).Returns(product);
            A.CallTo(() => productFixture.productRepository.UpdateItemAsync(productFixture.TestProduct1)).Returns(false);

            //Act
            var result = await productFixture.productService.ChangeProductInfoAsync(userId, productFixture.TestChange1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.ratingRepository.CreateAsync(A<ProductRating>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncNegative_InfoIsNull_ReturnServiceResultWithBadRequest()
        {

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var nullRating = new ProductRating();
            var nullProduct = new Product();

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(nullProduct.Id)).Returns(nullProduct);
            A.CallTo(() => productFixture.productRepository.UpdateItemAsync(productFixture.TestProduct1)).Returns(false);

            //Act
            var result = await productFixture.productService.ChangeProductInfoAsync(userId, null);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.productRepository.UpdateItemAsync(A<Product>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncNegative_InvalidId_ReturnServiceResultWithBadRequest()
        {

            int userId = 1;
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var nullProduct = new Product();

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(nullProduct.Id)).Returns(nullProduct);
            A.CallTo(() => productFixture.productRepository.UpdateItemAsync(productFixture.TestProduct1)).Returns(false);

            //Act
            var result = await productFixture.productService.ChangeProductInfoAsync(userId, productFixture.TestChange1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.productRepository.UpdateItemAsync(A<Product>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeProductInfoAsyncNegative_NullLogo_ReturnServiceResultWithBadRequest()
        {
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var nullProduct = new Product();

            A.CallTo(() => productFixture.productRepository.GetProductByIdAsync(productFixture.TestProduct3.Id)).Returns(productFixture.TestProduct3);
            A.CallTo(() => productFixture.productRepository.UpdateItemAsync(productFixture.TestProduct3)).Returns(false);
            A.CallTo(() => productFixture.cloudinaryService.DeleteImage(productFixture.TestProduct3.Logo)).Returns(serviceResult);
            //Act
            var result = await productFixture.productService.ChangeProductInfoAsync(2, productFixture.TestChange1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => productFixture.productRepository.UpdateItemAsync(A<Product>.Ignored)).MustNotHaveHappened();
        }
    }
}
