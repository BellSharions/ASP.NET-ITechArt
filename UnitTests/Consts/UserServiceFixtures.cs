using ASP_NET.Controllers.InformationControllers;
using AutoMapper;
using Business;
using Business.DTO;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Entities.Roles;
using DAL.Enums;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace UnitTests.Consts
{
    public class UserServiceFixtures
    {
        public User UserTest1 { get; set; }
        public User UserTest2 { get; set; }
        public User UserTest6 { get; set; }
        public User NullUser { get; set; }
        public CreateUserModel UserCreationTest1 { get; set; }
        public CreateUserModel UserCreationTest2 { get; set; }
        public CreateUserModel UserCreationTest4 { get; set; }
        public ChangeUserInfoDto UserChangeTest1 { get; set; }
        public ChangePasswordUserDto UserPasswordTest1 { get; set; }
        public UserInfoDto UserInfoTest1 { get; set; }
        public UserInfoDto UserInfoTest3 { get; set; }
        public ChangeUserInfoDto UserInfoTest2 { get; set; }
        public ChangePasswordUserDto UserPasswordTest2 { get; set; }
        public IUserRepository userRepository { get; set; }
        public ISmtpService smtpService { get; set; }
        public IMapper mapper { get; set; }
        public SignInManager<User> signInManager { get; set; }
        public UserManager<User> userManager { get; set; }
        public RoleManager<Role> roleManager { get; set; }
        public UserService userService { get; set; }
        public IMemoryCache memoryCache { get; set; }
        public IHttpContextAccessor httpContextAccessor { get; set; }
        public UserInformationController userController { get; set; }
        public JsonPatchDocument<ChangePasswordUserDto> jsonPatch { get; set; }
        public IUserService userControllerService { get; set; }
        public ServiceResult serviceResultOk { get; set; }
        public ServiceResult serviceResultBadRequest { get; set; }
        public ServiceResult serviceResultCreated { get; set; }

        public UserServiceFixtures()
        {
            UserTest1 = new()
            {
                Id = 5,
                UserName = "Bell",
                NormalizedUserName = "BELL",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
                PhoneNumber = "375293798389",
                PasswordHash = "Ugfkr23dff@FDDFFF"
            };
            UserTest2 = new()
            {
                Id = 5,
                UserName = "Bell",
                NormalizedUserName = "BELL",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                EmailConfirmed = false,
                NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
                PhoneNumber = "375293798389",
                PasswordHash = "Ugfkr23dff@FDDFFF"
            };
            UserTest6 = new()
            {
                Id = 5,
                UserName = "BelL",
                NormalizedUserName = "BELL",
                AdressDelivery = "tesT",
                Email = "bellsharions@gmail.com",
                EmailConfirmed = false,
                NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
                PhoneNumber = "375293798389",
                PasswordHash = "Ugfkr23dff@FDDFFF"
            };
            NullUser = null;
            UserCreationTest1 = new()
            {
                UserName = "Bell",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389",
                Password = "12345678Bb#"
            };
            UserCreationTest2 = new()
            {
                UserName = "Bell",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389",
                Password = "13345678Bb#"
            };
            UserCreationTest4 = new()
            {
                UserName = "Bell",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389",
                Password = "wrong"
            };
            UserChangeTest1 = new()
            {
                UserName = "BelL",
                AdressDelivery = "tesT",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389"
            };
            UserPasswordTest1 = new()
            {
                Id = 5,
                OldPassword = "13345678Bb#",
                NewPassword = "12345678Bb#"
            };
            UserInfoTest1 = new()
            {
                UserName = "Bell",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389"
            };
            UserInfoTest3 = new()
            {
                UserName = "BelL",
                AdressDelivery = "tesT",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389"
            };
            UserPasswordTest2 = new()
            {
                Id = 1,
                OldPassword = "test",
                NewPassword = "test2"
            };
            userRepository = A.Fake<IUserRepository>();
            smtpService = A.Fake<ISmtpService>();
            mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
            signInManager = A.Fake<SignInManager<User>>();
            userManager = A.Fake<UserManager<User>>();
            roleManager = A.Fake<RoleManager<Role>>();
            userService = new UserService(userManager, mapper, signInManager, userRepository, smtpService);
            memoryCache = A.Fake<IMemoryCache>();
            httpContextAccessor = A.Fake<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();
            userControllerService = A.Fake<IUserService>();
            userController = new UserInformationController(userControllerService, memoryCache, httpContextAccessor);
            jsonPatch = new JsonPatchDocument<ChangePasswordUserDto>();
            jsonPatch.Replace(j => j.Id, 1);
            jsonPatch.Replace(j => j.OldPassword, "test");
            jsonPatch.Replace(j => j.NewPassword, "test2");
            serviceResultOk = new ServiceResult(ResultType.Success, "Success");
            serviceResultBadRequest = new ServiceResult(ResultType.BadRequest, "Invalid Information");
            serviceResultCreated = new ServiceResult(ResultType.Created, "Created");
        }
    }
}
