using ASP_NET.Models;
using AutoMapper;
using Business.Interfaces;
using DAL.Entities;
using DAL.Entities.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly SmtpOptions _options;

        private readonly string emailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        private readonly string passwordRegex = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository userRepository, IOptions<SmtpOptions> SmtpOptionsAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _options = SmtpOptionsAccessor.Value;
            _mapper = mapper;
        }
        public async Task<int> ChangePasswordAsync(int id, JsonPatchDocument<User> patchDoc)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newUser = user;
            patchDoc.ApplyTo(newUser);
            if (!(await _userRepository.UpdatePasswordAsync(user, token, newUser.PasswordHash)))
                return 400;
            return 201;
        }

        public async Task<int> ConfirmEmailAsync(int id, string token)
        {
            if (!(_userManager.FindByIdAsync((id).ToString()).Result == null) &&
                !(await _userManager.ConfirmEmailAsync(_userManager.FindByIdAsync((id).ToString()).Result, token)).Succeeded)
                return 400;
            return 200;
        }

        public async Task<User> FindUserByIdAsync(int id) => await _userRepository.GetUserByIdAsync(id);

        public async Task<int> RegisterAsync(UserModel info)
        {
            if (!Regex.IsMatch(info.Email, emailRegex) &&
                !Regex.IsMatch(info.Password, passwordRegex) &&
                _userManager.FindByEmailAsync(info.Email).IsCompletedSuccessfully)
                return 400;
            User user = _mapper.Map<User>(info);
            user.UserName = info.Email;
            await _userManager.CreateAsync(user, info.Password);
            await _userManager.AddToRoleAsync(user, "User");
            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var callbackUrl = $"https://localhost:44343/api/auth/email-confirmation?id={user.Id}&token={token}";
            var sender = new MailSender(_options).SendAsync(info.Email, "Email confirmation", "Please use this link to confirm your email: " + callbackUrl);
            return 200;
        }

        public async Task<int> SigninAsync(UserModel info)
        {
            var foundUser = _userManager.FindByEmailAsync(info.Email)?.Result;
            if (!Regex.IsMatch(info.Email, emailRegex) &&
                !Regex.IsMatch(info.Password, passwordRegex) &&
                !foundUser.EmailConfirmed &&
                !await _userManager.CheckPasswordAsync(foundUser, info.Password))
                return 400;
            await _signInManager.SignInAsync(foundUser, true);
            return 200;
            }

        public async Task<User> UpdateUserInfoAsync(int id, UserModel user) => await _userRepository.UpdateUserInfoAsync(id, user);
    }
}
