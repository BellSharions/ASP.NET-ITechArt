using Business.DTO;
using DAL.Entities.Models;
using DAL.Enums;
using FakeItEasy;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var userInfo = userFixture.CorrectUserInfo;
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
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var userInfo = userFixture.CorrectUserInfo;
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
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var badresult = new BadRequestResult();
            var userInfo = userFixture.CorrectUserInfo;

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
            var user = userFixture.CorrectUserWithChangedInformation;
            var userInfo = userFixture.CorrectUserInfo;
            var userChange = userFixture.UserInformationToChange;
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
            var user = userFixture.CorrectUserWithChangedInformation;
            var userInfo = userFixture.CorrectUserInfo;
            var userChange = userFixture.UserInformationToChange;
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
            var user = userFixture.CorrectUserWithChangedInformation;
            var userInfo = userFixture.CorrectUserInfo;
            var userChange = userFixture.UserInformationToChange;
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
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var patchDoc = userFixture.jsonPatch;
            var okresult = new CreatedResult("", null);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var userPassword = userFixture.UserPasswordChange;

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
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var patchDoc = userFixture.jsonPatch;
            var badResult = new BadRequestObjectResult("");
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var userPassword = userFixture.UserPasswordChange;

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
            var user = userFixture.CorrectUserWithConfirmedEmail;
            JsonPatchDocument<ChangePasswordUserDto> patchDoc = null;
            var badResult = new BadRequestObjectResult("");
            var serviceResult = new ServiceResult(ResultType.BadRequest, "");
            var userPassword = userFixture.UserPasswordChange;

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
