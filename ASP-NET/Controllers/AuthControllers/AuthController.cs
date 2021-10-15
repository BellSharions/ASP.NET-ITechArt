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

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IOptions<SmtpOptions> SmtpOptionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _options = SmtpOptionsAccessor.Value;
        }

        [Authorize(Roles = "Admin")]
        [Route("create-roles")]
        [HttpGet]
        public async Task EnsureRolesCreated()
        {
            var adminRole = new Role("Admin", "Administrator");
            var userRole = new Role("User", "User");
            if (_roleManager.RoleExistsAsync(adminRole.Name).Result && _roleManager.RoleExistsAsync(userRole.Name).Result)
            {
                await _roleManager.CreateAsync(adminRole);
                await _roleManager.CreateAsync(userRole);
            }
        }

        [Route("sign-in")]
        [HttpPost]
        public async Task<IActionResult> SignIn()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEndAsync().Result.Split(" ");
                var email = body[0];
                var password = body[1];
                if (Regex.Match(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").Success &&
                    Regex.Match(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z]).*$").Success &&
                    _userManager.FindByEmailAsync(email).Result.EmailConfirmed)
                {
                    User user = new() {
                        Id = _userManager.FindByEmailAsync(email).Result.Id,
                        UserName = email,
                        Email = email,
                        SecurityStamp = Guid.NewGuid().ToString() };
                    await _userManager.UpdateSecurityStampAsync(user);
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
                    await _signInManager.SignInAsync(user, true);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [Route("sign-up")]
        [HttpPost]
        public async Task<IActionResult> SignUp()
        {
            using var reader = new StreamReader(Request.Body);
            var body = (reader.ReadToEndAsync()).Result.Split(" ");
            var email = body[0];
            var password = body[1];
            User user = new() { 
                UserName = email, 
                Email = email, 
                PasswordHash = password}; //change to work with view model instead of working with DAL entity? 
            if (Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$") && 
                Regex.IsMatch(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$") && 
                !_userManager.FindByEmailAsync(email).IsCompleted)
            {
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "User");
                var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
                var callbackUrl = $"https://localhost:44343/api/auth/email-confirmation?id={user.Id}&token={token}";
                var sender = new MailSender(_options).SendAsync(email, "Email confirmation", "Please use this link to confirm your email: " + callbackUrl);
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
