using AutoMapper;
using Business;
using Business.DTO;
using Business.Enums;
using Business.Interfaces;
using Business.Services;
using DAL.Enums;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.Consts;
using Xunit;

namespace UnitTests.ServiceTests
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task GetTopPlatforms_ReturnResultWithListOfTopPlatformDto()
        {
            var productRepository = A.Fake<IProductRepository>();
            var cloudinaryService = A.Fake<ICloudinaryService>();
            var ratingRepository = A.Fake<IRatingRepository>();
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();

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
    }
}
