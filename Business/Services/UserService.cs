﻿using ASP_NET.Models;
using Business.DTO;
using Business.Interfaces;
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
        private readonly SmtpOptions _options;

        private readonly string emailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        private readonly string passwordRegex = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository userRepository, IOptions<SmtpOptions> SmtpOptionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _options = SmtpOptionsAccessor.Value;
        }
        public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordUserDto user)
        {
            var newUser = await _userRepository.GetUser(user.Id);
            if (!(await _userRepository.UpdatePasswordAsync(user)))
                return new ServiceResult(ResultType.BadRequest, "Password was not updated");
            return new ServiceResult(ResultType.Created, "Password was not updated");
        }

        public async Task<ServiceResult> ConfirmEmailAsync(int id, string token)
        {
            if (!(_userManager.FindByIdAsync((id).ToString()).Result == null) &&
                !(await _userManager.ConfirmEmailAsync(_userManager.FindByIdAsync((id).ToString()).Result, token)).Succeeded)
                return new ServiceResult(ResultType.BadRequest, "Email was not confirmed");
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<User> FindUserByIdAsync(int id) => await _userRepository.GetUser(id);

        public async Task<ServiceResult> RegisterAsync(CreateUserModel info)
        {
            if (!Regex.IsMatch(info.Email, emailRegex) &&
                !Regex.IsMatch(info.Password, passwordRegex) &&
                _userManager.FindByEmailAsync(info.Email).IsCompletedSuccessfully)
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
            var sender = new MailSender(_options).SendAsync(info.Email, "Email confirmation", "Please use this link to confirm your email: " + callbackUrl);
            return new ServiceResult(ResultType.Success, "Success");
        }

        public async Task<ServiceResult> SigninAsync(CreateUserModel info)
        {
            var foundUser = _userManager.FindByEmailAsync(info.Email)?.Result;
            if (!Regex.IsMatch(info.Email, emailRegex) &&
                !Regex.IsMatch(info.Password, passwordRegex) &&
                !foundUser.EmailConfirmed &&
                !await _userManager.CheckPasswordAsync(foundUser, info.Password))
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
