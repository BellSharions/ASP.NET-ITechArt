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
using Swashbuckle.AspNetCore.Annotations;

namespace ASP_NET.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
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

        [HttpPost("sign-in")]
        [SwaggerOperation(
            Summary = "Signs in specified user",
            Description = "Requires email and pasword in json format",
            OperationId = "SignIn",
            Tags = new[] { "Authorization", "User" })]

        [SwaggerResponse(200, "User was signed in")]
        [SwaggerResponse(400, "User was not signed in due to invalid information")]
        public async Task<IActionResult> SignIn([FromBody, SwaggerParameter("Information containing email and password", Required = true)] UserModel info)
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

        [HttpPost("sign-up")]
        [SwaggerOperation(
            Summary = "Registeres new user",
            Description = "Requires email and pasword in json format",
            OperationId = "SignUp",
            Tags = new[] { "Authorization", "User" })]
        [SwaggerResponse(201, "User was created")]
        [SwaggerResponse(400, "User was not created due to invalid information")]
        public async Task<IActionResult> SignUp([FromBody, SwaggerParameter("Information containing email and password", Required = true)] UserModel info)
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

        [HttpGet("email-confirmation")]
        [SwaggerOperation(
            Summary = "Confirms email of registered user",
            Description = "Requires id and token provided by generated url",
            OperationId = "EmailConfirmation",
            Tags = new[] { "Authorization", "User" })]
        [SwaggerResponse(204, "Email was confirmed")]
        [SwaggerResponse(400, "Email was not confirmed due to incorrect user/token")]
        public async Task<IActionResult> EmailConfirm([SwaggerParameter("User ID", Required = true)]int id, [SwaggerParameter("Email confirmation token", Required = true)] string token)
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
