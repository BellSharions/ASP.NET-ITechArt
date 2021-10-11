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
using Business.Interfaces;
using Business.Repositories;
using DAL;

namespace ASP_NET.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        IRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly SmtpOptions _options;
        private readonly IMapper _mapper;
        private readonly string emailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        private readonly string passwordRegex = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";

        public AuthController(UserManager<User> userManager, ApplicationDbContext context, SignInManager<User> signInManager, IOptions<SmtpOptions> SmtpOptionsAccessor, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = SmtpOptionsAccessor.Value;
            _mapper = mapper;
            _userRepository = new UserRepository(context);
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
            if (!Regex.IsMatch(info.Email, emailRegex) && //have consts istead of strings
                !Regex.IsMatch(info.Password, passwordRegex) &&
                !foundUser.EmailConfirmed &&
                !await _userManager.CheckPasswordAsync(foundUser, info.Password))
                return BadRequest();
            await _signInManager.SignInAsync(foundUser, true);
            return Ok();
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
        {
            if (!Regex.IsMatch(info.Email, emailRegex) && 
                !Regex.IsMatch(info.Password, passwordRegex) && 
                _userManager.FindByEmailAsync(info.Email).IsCompletedSuccessfully &&
                !ModelState.IsValid)
                return BadRequest();
            User user = _mapper.Map<User>(info);
            user.UserName = info.Email;
            await _userManager.CreateAsync(user, info.Password);
            await _userManager.AddToRoleAsync(user, "User");
            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var callbackUrl = $"https://localhost:44343/api/auth/email-confirmation?id={user.Id}&token={token}";
            var sender = new MailSender(_options).SendAsync(info.Email, "Email confirmation", "Please use this link to confirm your email: " + callbackUrl);
            return Created("api/auth/sign-up", user);
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
            if (!(_userManager.FindByIdAsync((id).ToString()).Result == null) &&
                !(await _userManager.ConfirmEmailAsync(_userManager.FindByIdAsync((id).ToString()).Result, token)).Succeeded)
                return BadRequest();
            return NoContent();
        }
    }
}
