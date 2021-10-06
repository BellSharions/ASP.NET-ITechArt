using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;
using MimeKit;
using MailKit.Net.Smtp;
using Serilog;
using System;

namespace ASP_NET.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                if (Regex.Match(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").Success && Regex.Match(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z]).*$").Success)
                {
                    User user = new() { UserName = email, Email = email, PasswordHash = password };
                    await _signInManager.SignInAsync(user, false);
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
            User user = new() { UserName = email, Email = email, PasswordHash = password}; //change to work with view model instead of working with DAL entity
            if (Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$") && 
                Regex.IsMatch(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$") && 
                (await _userManager.CreateAsync(user, password)).Succeeded)
            {
                //cookies?
                await _signInManager.SignInAsync(user, false);
                return Created("api/auth/sign-up", user);
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("email-confirmation")]
        [HttpGet]
        public async Task<IActionResult> EmailConfirm(string email)
        {
            if (Regex.Match(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").Success)
            {
                var message = new MimeMessage();
                var fromEmail = "nreply92@mail.ru";
                message.From.Add(new MailboxAddress((string) null, fromEmail));
                message.To.Add(new MailboxAddress((string)null, email));
                message.Subject = "email confirmation";
                message.Body = new TextPart("plain") { Text = "this is an email confirmation" };
                try
                {
                    using var client = new SmtpClient();
                    await client.ConnectAsync("smtp.mail.ru", 465, true);
                    await client.AuthenticateAsync("nreply92", "Qk2FeydHVAMEqg9e3Erd");
                    await client.SendAsync(message).ConfigureAwait(false);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Message wasn't sent");
                }
                return NoContent();
            }
            else
                return BadRequest();
        }
    }
}
