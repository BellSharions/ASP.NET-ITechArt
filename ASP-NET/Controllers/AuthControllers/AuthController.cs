using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;
using System;
using Business;
using Microsoft.Extensions.Options;
using DAL.Entities.Roles;
using System.Web;
using ASP_NET.Models;
using AutoMapper;

namespace ASP_NET.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SmtpOptions _options;
        private readonly IMapper _mapper;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IOptions<SmtpOptions> SmtpOptionsAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _options = SmtpOptionsAccessor.Value;
            _mapper = mapper;
        }

        [Route("sign-in")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] UserModel info)
        {
            var foundUser = _userManager.FindByEmailAsync(info.Email)?.Result;
            if (Regex.Match(info.Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").Success &&
                    Regex.Match(info.Password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z]).*$").Success &&
                    foundUser.EmailConfirmed &&
                    await _userManager.CheckPasswordAsync(foundUser, info.Password))
            {
                await _signInManager.SignInAsync(foundUser, true);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("sign-up")]
        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] UserModel info)
        {//change to work with view model instead of working with DAL entity? 
            if (Regex.IsMatch(info.Email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$") && 
                Regex.IsMatch(info.Password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$") && 
                !_userManager.FindByEmailAsync(info.Email).IsCompletedSuccessfully &&
                ModelState.IsValid)
            { 
                User user = _mapper.Map<User>(info);
                user.UserName = info.Email;
                await _userManager.CreateAsync(user, info.Password);
                await _userManager.AddToRoleAsync(user, "User");
                var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
                var callbackUrl = $"https://localhost:44343/api/auth/email-confirmation?id={user.Id}&token={token}";
                var sender = new MailSender(_options).SendAsync(info.Email, "Email confirmation", "Please use this link to confirm your email: " + callbackUrl);
                return Created("api/auth/sign-up", user);
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("email-confirmation")]
        [HttpGet]
        public async Task<IActionResult> EmailConfirm(int id, string token)
        {
            if (_userManager.FindByIdAsync((id).ToString()).Result != null &&
                (await _userManager.ConfirmEmailAsync(_userManager.FindByIdAsync((id).ToString()).Result, token)).Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
