using ASP_NET.Controllers.InformationControllers;
using AutoMapper;
using Business;
using Business.DTO;
using Business.Interfaces;
using Business.Services;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Entities.Roles;
using DAL.Enums;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Consts;
using Xunit;

namespace UnitTests.ControllerTests
{
    public class UserInformationControllerTest : IClassFixture<UserServiceFixtures>
    {
        UserServiceFixtures userFixture;
        public UserInformationControllerTest(UserServiceFixtures fixture)
        {
            this.userFixture = fixture;
            Fake.ClearRecordedCalls(userFixture.memoryCache);
            Fake.ClearRecordedCalls(userFixture.httpContextAccessor);
            Fake.ClearRecordedCalls(userFixture.userControllerService);
        }
        [Fact]
        public async Task GetUserPositive_NoCache_ReturnUserInfoDto()
        {
            var user = userFixture.UserTest1;
            var userInfo = userFixture.UserInfoTest1;
            var okresult = new OkObjectResult(userInfo);

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userFixture.userControllerService.FindUserInfoByIdAsync(user.Id)).Returns(userInfo);

            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userFixture.userController.GetUser();

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userFixture.userControllerService.FindUserInfoByIdAsync(user.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetUserPositive_HasCache_ReturnUserInfoDto()
        {
            var user = userFixture.UserTest1;
            var userInfo = userFixture.UserInfoTest1;
            var okresult = new OkObjectResult(userInfo);
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromDays(5)
            };
            userFixture.memoryCache.Set(int.Parse(user.Id.ToString()), user, cacheExpiryOptions);

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userFixture.userControllerService.FindUserInfoByIdAsync(user.Id)).Returns(userInfo);

            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userFixture.userController.GetUser();

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userFixture.userControllerService.FindUserInfoByIdAsync(user.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetUserNegative_ReturnBadRequest()
        {
            var user = userFixture.UserTest1;
            var badresult = new BadRequestResult();
            var userInfo = userFixture.UserInfoTest1;

            var userIdClaim = A.Fake<Claim>();
            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });

            A.CallTo(() => userFixture.userControllerService.FindUserInfoByIdAsync(user.Id)).Returns(userInfo);

            //Act
            var result = await userFixture.userController.GetUser();

            //Assert
            Assert.Equal(result.GetType(), badresult.GetType());

            A.CallTo(() => userFixture.userControllerService.FindUserInfoByIdAsync(user.Id)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeUserInfoPositive_ReturnOk()
        {
            var user = userFixture.UserTest6;
            var userInfo = userFixture.UserInfoTest3;
            var userChange = userFixture.UserChangeTest1;
            var okresult = new OkResult();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userFixture.userControllerService.UpdateUserInfoAsync(user.Id, userChange)).Returns(user);

            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userFixture.userController.ChangeUserInfo(userChange);

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userFixture.userControllerService.UpdateUserInfoAsync(user.Id, userChange)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeUserInfoNegative_NoUser_ReturnBadRequest()
        {
            var user = userFixture.UserTest6;
            var userInfo = userFixture.UserInfoTest3;
            var userChange = userFixture.UserChangeTest1;
            var okresult = new BadRequestResult();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, "")));

            A.CallTo(() => userFixture.userControllerService.UpdateUserInfoAsync(user.Id, userChange)).Returns(user);

            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userFixture.userController.ChangeUserInfo(userChange);

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userFixture.userControllerService.UpdateUserInfoAsync(user.Id, userChange)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeUserInfoNegative_ResultIsNull_ReturnBadRequest()
        {
            var user = userFixture.UserTest6;
            var userInfo = userFixture.UserInfoTest3;
            var userChange = userFixture.UserChangeTest1;
            var nullUser = userFixture.NullUser;
            var okresult = new BadRequestResult();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userFixture.userControllerService.UpdateUserInfoAsync(user.Id, userChange)).Returns(nullUser);

            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userFixture.userController.ChangeUserInfo(userChange);

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userFixture.userControllerService.UpdateUserInfoAsync(user.Id, userChange)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeUserPasswordPositive_ReturnOk()
        {
            var user = userFixture.UserTest6;
            var patchDoc = userFixture.jsonPatch;
            var okresult = new CreatedResult("", null);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var userPassword = userFixture.UserPasswordTest2;

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userFixture.userControllerService.ChangePasswordAsync(A<ChangePasswordUserDto>.Ignored)).Returns(serviceResult);

            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userFixture.userController.ChangeUserPassword(patchDoc);

            //Assert
            Assert.Equal(okresult.GetType(), result.GetType());

            A.CallTo(() => userFixture.userControllerService.ChangePasswordAsync(A<ChangePasswordUserDto>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeUserPasswordNegative_NoUser_ReturnBadRequest()
        {
            var user = userFixture.UserTest6;
            var patchDoc = userFixture.jsonPatch;
            var badResult = new BadRequestObjectResult("");
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var userPassword = userFixture.UserPasswordTest2;

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, "")));

            A.CallTo(() => userFixture.userControllerService.ChangePasswordAsync(userPassword)).Returns(serviceResult);

            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userFixture.userController.ChangeUserPassword(patchDoc);

            //Assert
            Assert.Equal(result.GetType(), badResult.GetType());

            A.CallTo(() => userFixture.userControllerService.ChangePasswordAsync(A<ChangePasswordUserDto>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeUserPasswordNegative_PatchDocIsNull_ReturnBadRequest()
        {
            var user = userFixture.UserTest6;
            JsonPatchDocument<ChangePasswordUserDto> patchDoc = null;
            var badResult = new BadRequestObjectResult("");
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var userPassword = userFixture.UserPasswordTest2;

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, "")));

            A.CallTo(() => userFixture.userControllerService.ChangePasswordAsync(userPassword)).Returns(serviceResult);

            A.CallTo(() => userFixture.httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userFixture.userController.ChangeUserPassword(patchDoc);

            //Assert
            Assert.Equal(result.GetType(), badResult.GetType());

            A.CallTo(() => userFixture.userControllerService.ChangePasswordAsync(A<ChangePasswordUserDto>.Ignored)).MustNotHaveHappened();
        }
    }
}
