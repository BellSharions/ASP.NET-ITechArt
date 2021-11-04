using System;
using Xunit;
using FakeItEasy;
using Business.Interfaces;
using System.Threading.Tasks;
using Business.Services;
using AutoMapper;
using Business;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;
using DAL.Entities.Roles;
using UnitTests.Consts;
using DAL.Entities.Models;
using DAL.Enums;
using Business.DTO;

namespace UnitTests
{
    public class UserServiceTest
    {
        public IUserRepository userRepository = A.Fake<IUserRepository>();
        public ISmtpService smtpService = A.Fake<ISmtpService>();
        public IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
        public SignInManager<User> signInManager = A.Fake<SignInManager<User>>();
        public UserManager<User> userManager = A.Fake<UserManager<User>>();
        public RoleManager<Role> roleManager = A.Fake<RoleManager<Role>>();
        [Fact]
        public async Task FindUserByIdPositive_ReturnUser()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);

            var user = Users.UserTest1;

            A.CallTo(() => userRepository.GetUser(user.Id)).Returns(user);

            //Act
            var result = await userService.FindUserByIdAsync(user.Id);

            //Assert
            Assert.True(result.Equals(user));

            A.CallTo(() => userRepository.GetUser(user.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task FindUserInfoByIdPositive_ReturnUser()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);

            var user = Users.UserTest2;

            A.CallTo(() => userRepository.GetUser(user.Id)).Returns(user);

            //Act
            var result = await userService.FindUserInfoByIdAsync(user.Id);

            //Assert
            Assert.True(result.Equals(Users.UserInfoTest1));

            A.CallTo(() => userRepository.GetUser(user.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangePasswordAsyncPositive_ReturnServiceResultWithCreated()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.Created, "Password was updated");
            var user = Users.UserTest2;

            A.CallTo(() => userRepository.UpdatePasswordAsync(Users.UserPasswordTest1)).Returns(true);

            //Act
            var result = await userService.ChangePasswordAsync(Users.UserPasswordTest1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userRepository.UpdatePasswordAsync(Users.UserPasswordTest1)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangePasswordAsyncNegative_ReturnServiceResultWithBadRequest()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Password was not updated");
            var user = Users.UserTest2;

            A.CallTo(() => userRepository.UpdatePasswordAsync(Users.UserPasswordTest1)).Returns(true);

            //Act
            var result = await userService.ChangePasswordAsync(new ChangePasswordUserDto());

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userRepository.UpdatePasswordAsync(Users.UserPasswordTest1)).MustNotHaveHappened();
        }
        [Fact]
        public async Task SigninAsyncPositive_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var user = Users.UserTest4;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).Returns(user);
            A.CallTo(() => userManager.CheckPasswordAsync(user, "12345678Bb#")).Returns(true);

            //Act
            var result = await userService.SigninAsync(Users.UserCreationTest1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.CheckPasswordAsync(user, "12345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task SigninAsyncNegative_NoUser_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");
            var nullUser = new User();
            var user = Users.UserTest4;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).Returns(nullUser);
            A.CallTo(() => userManager.CheckPasswordAsync(user, "12345678Bb#")).Returns(false);

            //Act
            var result = await userService.SigninAsync(Users.UserCreationTest1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.CheckPasswordAsync(nullUser, "12345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task SigninAsyncNegative_WrongPassword_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");
            var user = Users.UserTest4;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).Returns(user);
            A.CallTo(() => userManager.CheckPasswordAsync(user, "13345678Bb#")).Returns(false);

            //Act
            var result = await userService.SigninAsync(Users.UserCreationTest2);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.CheckPasswordAsync(user, "13345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task SigninAsyncNegative_WrongEmail_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");
            var user = Users.UserTest4;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email + "m")).Returns(user);
            A.CallTo(() => userManager.CheckPasswordAsync(A<User>.Ignored, "13345678Bb#")).Returns(false);

            //Act
            var result = await userService.SigninAsync(Users.UserCreationTest2);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email + "m")).MustNotHaveHappened();
            A.CallTo(() => userManager.CheckPasswordAsync(A<User>.Ignored, "13345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task SigninAsyncNegative_EmailConfirmedIsFalse_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");
            var user = Users.UserTest5;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).Returns(user);
            A.CallTo(() => userManager.CheckPasswordAsync(user, "13345678Bb#")).Returns(true);

            //Act
            var result = await userService.SigninAsync(Users.UserCreationTest2);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.CheckPasswordAsync(A<User>.Ignored, "13345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RegisterAsyncPositive_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var nullUser = new User();
            var user = Users.UserTest4;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).Returns(nullUser);

            //Act
            var result = await userService.RegisterAsync(Users.UserCreationTest1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RegisterAsyncNegative_FoundUser_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");
            var user = Users.UserTest4;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).Returns(user);

            //Act
            var result = await userService.RegisterAsync(Users.UserCreationTest1);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RegisterAsyncNegative_WrongPassword_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");
            var user = Users.UserTest4;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).Returns(user);

            //Act
            var result = await userService.RegisterAsync(Users.UserCreationTest4);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RegisterAsyncNegative_WrongEmail_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Invalid information");
            var user = Users.UserTest4;

            A.CallTo(() => userManager.FindByEmailAsync(user.Email + "m")).Returns(user);

            //Act
            var result = await userService.RegisterAsync(Users.UserCreationTest3);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByEmailAsync(user.Email + "m")).MustNotHaveHappened();
        }
        [Fact]
        public async Task ConfirmEmailAsyncPositive_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var nullUser = new User();
            var user = Users.UserTest4;
            var token = "testToken";

            A.CallTo(() => userManager.FindByIdAsync(user.Id.ToString())).Returns(user);
            A.CallTo(() => userManager.ConfirmEmailAsync(user, token)).Returns(IdentityResult.Success);

            //Act
            var result = await userService.ConfirmEmailAsync(user.Id, token);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByIdAsync(user.Id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.ConfirmEmailAsync(user, token)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ConfirmEmailAsyncNegative_NoUser_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.BadRequest, "Email was not confirmed");
            var nullUser = new User();
            var user = Users.UserTest4;
            var token = "testToken";

            A.CallTo(() => userManager.FindByIdAsync(user.Id.ToString())).Returns(nullUser);
            A.CallTo(() => userManager.ConfirmEmailAsync(user, token)).Returns(IdentityResult.Success);
            A.CallTo(() => userManager.ConfirmEmailAsync(nullUser, token)).Returns(IdentityResult.Failed());

            //Act
            var result = await userService.ConfirmEmailAsync(user.Id, token);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByIdAsync(user.Id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.ConfirmEmailAsync(user, token)).MustNotHaveHappened();
            A.CallTo(() => userManager.ConfirmEmailAsync(nullUser, token)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ConfirmEmailAsyncNegative_WrongToken_ReturnServiceResultWithOk()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var nullUser = new User();
            var user = Users.UserTest4;
            var token = "testToken";
            var wrongToken = "Token";

            A.CallTo(() => userManager.FindByIdAsync(user.Id.ToString())).Returns(user);
            A.CallTo(() => userManager.ConfirmEmailAsync(user, token)).Returns(IdentityResult.Success);
            A.CallTo(() => userManager.ConfirmEmailAsync(user, wrongToken)).Returns(IdentityResult.Failed());

            //Act
            var result = await userService.ConfirmEmailAsync(user.Id, wrongToken);

            //Assert
            Assert.Equal(serviceResult.Type, result.Type);

            A.CallTo(() => userManager.FindByIdAsync(user.Id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.ConfirmEmailAsync(user, token)).MustNotHaveHappened();
            A.CallTo(() => userManager.ConfirmEmailAsync(user, wrongToken)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task UpdateUserInfoAsyncPositive_ReturnServiceResultWithUser()
        {
            var userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var nullUser = new User();
            var user = Users.UserTest5;
            var newUser = Users.UserTest6;

            A.CallTo(() => userManager.FindByIdAsync(user.Id.ToString())).Returns(user);
            A.CallTo(() => userRepository.UpdateUserInfoAsync(user)).Returns(newUser);

            //Act
            var result = await userService.UpdateUserInfoAsync(user.Id, Users.UserChangeTest1);

            //Assert
            Assert.True(result.Equals(newUser));

            A.CallTo(() => userManager.FindByIdAsync(user.Id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => userRepository.UpdateUserInfoAsync(user)).MustHaveHappenedOnceExactly();
        }
    }
}
