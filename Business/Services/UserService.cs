using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Business.Models;
using DAL.Entities;
using DAL.Entities.Models;
using DAL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly ISmtpService _smtpService;
        private readonly IMapper _mapper;

        private readonly string emailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        private readonly string passwordRegex = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
        public UserService(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IUserRepository userRepository, ISmtpService smtpService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _smtpService = smtpService;
            _mapper = mapper;
        }
        public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordUserDto user)
        {
            if (!(await _userRepository.UpdatePasswordAsync(user)))
                return new ServiceResult(ResultType.BadRequest, "Password was not updated");
            return new ServiceResult(ResultType.Created, "Password was updated");
        }

        public async Task<ServiceResult> ConfirmEmailAsync(int id, string token)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var confirmationResult = await _userManager.ConfirmEmailAsync(user, token);
            if (user == null &&
                !confirmationResult.Succeeded)
                return new ServiceResult(ResultType.BadRequest, "Email was not confirmed");
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<User> FindUserByIdAsync(int id) => await _userRepository.GetUser(id);
        public async Task<UserInfoDto> FindUserInfoByIdAsync(int id) 
        { 
            var queryResult = await _userRepository.GetUser(id);
            var result = new UserInfoDto();
            _mapper.Map(queryResult, result);
            return result;
        }

        public async Task<ServiceResult> RegisterAsync(CreateUserModel info)
        {
            var foundUser = await _userManager.FindByEmailAsync(info.Email);
            if (!Regex.IsMatch(info.Email, emailRegex) ||
                !Regex.IsMatch(info.Password, passwordRegex) ||
                foundUser != null)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");
            User user = new User() { 
                Email = info.Email, 
                UserName = info.Email, 
                PhoneNumber = info.PhoneNumber, 
                AdressDelivery = info.AdressDelivery
            };
            await _userManager.CreateAsync(user, info.Password);
            await _userManager.AddToRoleAsync(user, "User");
            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var callbackUrl = $"https://localhost:44343/api/auth/email-confirmation?id={user.Id}&token={token}";
            await _smtpService.SendAsync(info.Email, "Email confirmation", "Please use this link to confirm your email: " + callbackUrl);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ServiceResult> SigninAsync(CreateUserModel info)
        {
            var foundUser = await _userManager.FindByEmailAsync(info.Email);
            var passwordCheck = await _userManager.CheckPasswordAsync(foundUser, info.Password);
            if (foundUser == null ||
                !Regex.IsMatch(info.Email, emailRegex) ||
                !Regex.IsMatch(info.Password, passwordRegex) ||
                !foundUser.EmailConfirmed ||
                !passwordCheck)
                return new ServiceResult(ResultType.BadRequest, "Invalid information");

            await _signInManager.SignInAsync(foundUser, true);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<User> UpdateUserInfoAsync(int id, ChangeUserInfoDto user) 
        {
            var foundUser = await _userManager.FindByIdAsync(id.ToString());
            foundUser.UserName = user.UserName;
            foundUser.Email = user.Email;
            foundUser.PhoneNumber = user.PhoneNumber;
            foundUser.AdressDelivery = user.AdressDelivery;
            var result = await _userRepository.UpdateUserInfoAsync(foundUser); 
            return result;
        }
    }
}
