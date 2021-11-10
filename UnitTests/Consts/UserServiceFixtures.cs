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
        public User CorrectUserWithConfirmedEmail { get; set; }
        public User CorrectUserWithNotConfirmedEmail { get; set; }
        public User CorrectUserWithChangedInformation { get; set; }
        public User NullUser { get; set; }
        public CreateUserModel UserCreationTest1 { get; set; }
        public CreateUserModel UserWithChangedPassword { get; set; }
        public CreateUserModel UserWithWrongPassword { get; set; }
        public ChangeUserInfoDto UserInformationToChange { get; set; }
        public ChangePasswordUserDto UserPasswordChange { get; set; }
        public UserInfoDto CorrectUserInfo { get; set; }
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
            CorrectUserWithConfirmedEmail = new()
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
            CorrectUserWithNotConfirmedEmail = new()
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
            CorrectUserWithChangedInformation = new()
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
            UserWithChangedPassword = new()
            {
                UserName = "Bell",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389",
                Password = "13345678Bb#"
            };
            UserWithWrongPassword = new()
            {
                UserName = "Bell",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389",
                Password = "wrong"
            };
            UserInformationToChange = new()
            {
                UserName = "BelL",
                AdressDelivery = "tesT",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389"
            };
            UserPasswordChange = new()
            {
                Id = 1,
                OldPassword = "13345678Bb#",
                NewPassword = "12345678Bb#"
            };
            CorrectUserInfo = new()
            {
                UserName = "Bell",
                AdressDelivery = "test",
                Email = "bellsharions@gmail.com",
                PhoneNumber = "375293798389"
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
