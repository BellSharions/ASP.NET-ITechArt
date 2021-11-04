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
    public class UserInformationControllerTest
    {
        public IUserService userService = A.Fake<IUserService>();
        public IMemoryCache memoryCache = A.Fake<IMemoryCache>();
        public IHttpContextAccessor httpContextAccessor = A.Fake<IHttpContextAccessor>();
        [Fact]
        public async Task GetUserPositive_NoCache_ReturnUserInfoDto()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);

            var user = Users.UserTest1;
            var userInfo = Users.UserInfoTest1;
            var okresult = new OkObjectResult(userInfo);
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userService.FindUserInfoByIdAsync(user.Id)).Returns(userInfo);

            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userController.GetUser();

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userService.FindUserInfoByIdAsync(user.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetUserPositive_HasCache_ReturnUserInfoDto()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);

            var user = Users.UserTest1;
            var userInfo = Users.UserInfoTest1;
            var okresult = new OkObjectResult(userInfo);
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromDays(5)
            };
            memoryCache.Set(int.Parse(user.Id.ToString()), user, cacheExpiryOptions);

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userService.FindUserInfoByIdAsync(user.Id)).Returns(userInfo);

            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userController.GetUser();

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userService.FindUserInfoByIdAsync(user.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetUserNegative_ReturnBadRequest()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);

            var user = Users.UserTest1;
            var badresult = new BadRequestResult();
            var userInfo = Users.UserInfoTest1;

            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();
            var userIdClaim = A.Fake<Claim>();
            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });

            A.CallTo(() => userService.FindUserInfoByIdAsync(user.Id)).Returns(userInfo);

            //Act
            var result = await userController.GetUser();

            //Assert
            Assert.Equal(result.GetType(), badresult.GetType());

            A.CallTo(() => userService.FindUserInfoByIdAsync(user.Id)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeUserInfoPositive_ReturnOk()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);

            var user = Users.UserTest6;
            var userInfo = Users.UserInfoTest3;
            var userChange = Users.UserChangeTest1;
            var okresult = new OkResult();
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userService.UpdateUserInfoAsync(user.Id, userChange)).Returns(user);

            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userController.ChangeUserInfo(userChange);

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userService.UpdateUserInfoAsync(user.Id, userChange)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeUserInfoNegative_NoUser_ReturnBadRequest()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);

            var user = Users.UserTest6;
            var userInfo = Users.UserInfoTest3;
            var userChange = Users.UserChangeTest1;
            var okresult = new BadRequestResult();
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, "")));

            A.CallTo(() => userService.UpdateUserInfoAsync(user.Id, userChange)).Returns(user);

            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userController.ChangeUserInfo(userChange);

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userService.UpdateUserInfoAsync(user.Id, userChange)).MustNotHaveHappened();
        }
        [Fact]
        public async Task ChangeUserInfoNegative_ResultIsNull_ReturnBadRequest()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);

            var user = Users.UserTest6;
            var userInfo = Users.UserInfoTest3;
            var userChange = Users.UserChangeTest1;
            var nullUser = new User();
            var okresult = new BadRequestResult();
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userService.UpdateUserInfoAsync(user.Id, userChange)).Returns(nullUser);

            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userController.ChangeUserInfo(userChange);

            //Assert
            Assert.Equal(result.GetType(), okresult.GetType());

            A.CallTo(() => userService.UpdateUserInfoAsync(user.Id, userChange)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeUserPasswordPositive_ReturnOk()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);
            var user = Users.UserTest6;
            var nullUser = new ChangePasswordUserDto();
            var patchDoc = Users.GetJsonPatchDocumentForResetPassword();
            var okresult = new CreatedResult("", null);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var userPassword = Users.UserPasswordTest2;
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())));

            A.CallTo(() => userService.ChangePasswordAsync(A<ChangePasswordUserDto>.Ignored)).Returns(serviceResult);

            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userController.ChangeUserPassword(patchDoc);

            //Assert
            Assert.Equal(okresult.GetType(), result.GetType());

            A.CallTo(() => userService.ChangePasswordAsync(A<ChangePasswordUserDto>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeUserPasswordNegative_NoUser_ReturnBadRequest()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);

            var user = Users.UserTest6;
            var nullUser = new ChangePasswordUserDto();
            var patchDoc = Users.GetJsonPatchDocumentForResetPassword();
            var badResult = new BadRequestObjectResult("");
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var userPassword = Users.UserPasswordTest2;
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, "")));

            A.CallTo(() => userService.ChangePasswordAsync(userPassword)).Returns(serviceResult);

            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userController.ChangeUserPassword(patchDoc);

            //Assert
            Assert.Equal(result.GetType(), badResult.GetType());

            A.CallTo(() => userService.ChangePasswordAsync(A<ChangePasswordUserDto>.Ignored)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeUserPasswordNegative_PatchDocIsNull_ReturnBadRequest()
        {
            var userController = new UserInformationController(userService, memoryCache, httpContextAccessor);

            var user = Users.UserTest6;
            var nullUser = new ChangePasswordUserDto();
            JsonPatchDocument<ChangePasswordUserDto> patchDoc = null;
            var badResult = new BadRequestObjectResult("");
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var userPassword = Users.UserPasswordTest2;
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();

            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(ClaimTypes.NameIdentifier, "")));

            A.CallTo(() => userService.ChangePasswordAsync(userPassword)).Returns(serviceResult);

            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim });
            //Act
            var result = await userController.ChangeUserPassword(patchDoc);

            //Assert
            Assert.Equal(result.GetType(), badResult.GetType());

            A.CallTo(() => userService.ChangePasswordAsync(A<ChangePasswordUserDto>.Ignored)).MustNotHaveHappened();
        }
    }
}
