using Xunit;
using FakeItEasy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;
using UnitTests.Consts;
using DAL.Entities.Models;
using DAL.Enums;
using Business.DTO;

namespace UnitTests
{
    public class UserServiceTest : IClassFixture<UserServiceFixtures>
    {
        UserServiceFixtures userFixture;
        public UserServiceTest(UserServiceFixtures fixture)
        {
            this.userFixture = fixture;
            Fake.ClearRecordedCalls(userFixture.roleManager);
            Fake.ClearRecordedCalls(userFixture.signInManager);
            Fake.ClearRecordedCalls(userFixture.userRepository);
            Fake.ClearRecordedCalls(userFixture.smtpService);
            Fake.ClearRecordedCalls(userFixture.userManager);
        }
        [Fact]
        public async Task FindUserByIdPositive_ReturnUser()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userRepository.GetUser(user.Id)).Returns(user);

            //Act
            var result = await userFixture.userService.FindUserByIdAsync(user.Id);

            //Assert
            Assert.True(result.Equals(user));

            A.CallTo(() => userFixture.userRepository.GetUser(user.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task FindUserInfoByIdPositive_ReturnUser()
        {
            var user = userFixture.CorrectUserWithNotConfirmedEmail;

            A.CallTo(() => userFixture.userRepository.GetUser(user.Id)).Returns(user);

            //Act
            var result = await userFixture.userService.FindUserInfoByIdAsync(user.Id);

            //Assert
            Assert.True(result.Equals(userFixture.CorrectUserInfo));

            A.CallTo(() => userFixture.userRepository.GetUser(user.Id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangePasswordAsyncPositive_ReturnServiceResultWithCreated()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userRepository.UpdatePasswordAsync(userFixture.UserPasswordChange)).Returns(true);

            //Act
            var result = await userFixture.userService.ChangePasswordAsync(userFixture.UserPasswordChange);

            //Assert
            Assert.Equal(userFixture.serviceResultCreated.Type, result.Type);

            A.CallTo(() => userFixture.userRepository.UpdatePasswordAsync(userFixture.UserPasswordChange)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangePasswordAsyncNegative_ReturnServiceResultWithBadRequest()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userRepository.UpdatePasswordAsync(userFixture.UserPasswordChange)).Returns(true);

            //Act
            var result = await userFixture.userService.ChangePasswordAsync(new ChangePasswordUserDto());

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userRepository.UpdatePasswordAsync(userFixture.UserPasswordChange)).MustNotHaveHappened();
        }
        [Fact]
        public async Task SigninAsyncPositive_ReturnServiceResultWithOk()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).Returns(user);
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(user, "12345678Bb#")).Returns(true);

            //Act
            var result = await userFixture.userService.SigninAsync(userFixture.UserCreationTest);

            //Assert
            Assert.Equal(userFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(user, "12345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task SigninAsyncNegative_NoUser_ReturnServiceResultWithOk()
        {
            User nullUser = null;
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).Returns(nullUser);
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(user, "12345678Bb#")).Returns(false);

            //Act
            var result = await userFixture.userService.SigninAsync(userFixture.UserCreationTest);

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(nullUser, "12345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task SigninAsyncNegative_WrongPassword_ReturnServiceResultWithOk()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).Returns(user);
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(user, "wrong")).Returns(false);

            //Act
            var result = await userFixture.userService.SigninAsync(userFixture.UserWithWrongPassword);

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(user, "wrong")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task SigninAsyncNegative_WrongEmail_ReturnServiceResultWithOk()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email + "m")).Returns(user);
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(A<User>.Ignored, "13345678Bb#")).Returns(false);

            //Act
            var result = await userFixture.userService.SigninAsync(userFixture.UserWithChangedPassword);

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email + "m")).MustNotHaveHappened();
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(A<User>.Ignored, "13345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task SigninAsyncNegative_EmailConfirmedIsFalse_ReturnServiceResultWithOk()
        {
            var user = userFixture.CorrectUserWithNotConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).Returns(user);
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(user, "13345678Bb#")).Returns(true);

            //Act
            var result = await userFixture.userService.SigninAsync(userFixture.UserWithChangedPassword);

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userFixture.userManager.CheckPasswordAsync(A<User>.Ignored, "13345678Bb#")).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RegisterAsyncPositive_ReturnServiceResultWithOk()
        {
            var serviceResult = new ServiceResult(ResultType.Success, "Success");
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).Returns(userFixture.NullUser);

            //Act
            var result = await userFixture.userService.RegisterAsync(userFixture.UserCreationTest);

            //Assert
            Assert.Equal(userFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RegisterAsyncNegative_FoundUser_ReturnServiceResultWithOk()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).Returns(user);

            //Act
            var result = await userFixture.userService.RegisterAsync(userFixture.UserCreationTest);

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RegisterAsyncNegative_WrongPassword_ReturnServiceResultWithOk()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).Returns(user);

            //Act
            var result = await userFixture.userService.RegisterAsync(userFixture.UserWithWrongPassword);

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RegisterAsyncNegative_WrongEmail_ReturnServiceResultWithOk()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email + "m")).Returns(user);

            //Act
            var result = await userFixture.userService.RegisterAsync(userFixture.UserWithChangedPassword);

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByEmailAsync(user.Email + "m")).MustNotHaveHappened();
        }
        [Fact]
        public async Task ConfirmEmailAsyncPositive_ReturnServiceResultWithOk()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var token = "testToken";

            A.CallTo(() => userFixture.userManager.FindByIdAsync(user.Id.ToString())).Returns(user);
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(user, token)).Returns(IdentityResult.Success);

            //Act
            var result = await userFixture.userService.ConfirmEmailAsync(user.Id, token);

            //Assert
            Assert.Equal(userFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByIdAsync(user.Id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(user, token)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ConfirmEmailAsyncNegative_NoUser_ReturnServiceResultWithOk()
        {
            var nullUser = userFixture.NullUser;
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var token = "testToken";

            A.CallTo(() => userFixture.userManager.FindByIdAsync(user.Id.ToString())).Returns(nullUser);
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(user, token)).Returns(IdentityResult.Success);
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(nullUser, token)).Returns(IdentityResult.Failed());

            //Act
            var result = await userFixture.userService.ConfirmEmailAsync(user.Id, token);

            //Assert
            Assert.Equal(userFixture.serviceResultBadRequest.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByIdAsync(user.Id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(user, token)).MustNotHaveHappened();
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(nullUser, token)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ConfirmEmailAsyncNegative_WrongToken_ReturnServiceResultWithOk()
        {
            var nullUser = userFixture.NullUser;
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var token = "testToken";
            var wrongToken = "Token";

            A.CallTo(() => userFixture.userManager.FindByIdAsync(user.Id.ToString())).Returns(user);
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(user, token)).Returns(IdentityResult.Success);
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(user, wrongToken)).Returns(IdentityResult.Failed());

            //Act
            var result = await userFixture.userService.ConfirmEmailAsync(user.Id, wrongToken);

            //Assert
            Assert.Equal(userFixture.serviceResultOk.Type, result.Type);

            A.CallTo(() => userFixture.userManager.FindByIdAsync(user.Id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(user, token)).MustNotHaveHappened();
            A.CallTo(() => userFixture.userManager.ConfirmEmailAsync(user, wrongToken)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task UpdateUserInfoAsyncPositive_ReturnServiceResultWithUser()
        {
            var user = userFixture.CorrectUserWithConfirmedEmail;
            var newUser = userFixture.CorrectUserWithChangedInformation;

            A.CallTo(() => userFixture.userManager.FindByIdAsync(user.Id.ToString())).Returns(user);
            A.CallTo(() => userFixture.userRepository.UpdateUserInfoAsync(user)).Returns(newUser);

            //Act
            var result = await userFixture.userService.UpdateUserInfoAsync(user.Id, userFixture.UserInformationToChange);

            //Assert
            Assert.True(result.Equals(newUser));

            A.CallTo(() => userFixture.userManager.FindByIdAsync(user.Id.ToString())).MustHaveHappenedOnceExactly();
            A.CallTo(() => userFixture.userRepository.UpdateUserInfoAsync(user)).MustHaveHappenedOnceExactly();
        }
    }
}
