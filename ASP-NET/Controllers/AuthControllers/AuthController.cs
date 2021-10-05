using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ASP_NET.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        [Route("sign-in")]
        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            if (email == "")
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [Route("sign-up")]
        [HttpPost]
        public async Task<IActionResult> SignUp(string email, string password)
        {
            //validate email/password
            if (Regex.Match(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").Success && Regex.Match(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z]).*$").Success)
            {
                return Ok();
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
                //use smtp to send email confirmation
                //SmtpClient client = new SmtpClient(HttpContext.Request.Host.Host, (int)HttpContext.Request.Host.Port);
                //MailAddress from = new MailAddress("bellsharions@gmail.com");
                //MailAddress to = new MailAddress("kingofnarni@gmail.com");
                //MailMessage message = new MailMessage(from, to);
                //message.Body = "This is a test email message sent by an application. ";
                //message.BodyEncoding = System.Text.Encoding.UTF8;
                //message.Subject = "test message 1";
                //message.SubjectEncoding = System.Text.Encoding.UTF8;
                //string userState = "test message1";
                //client.SendAsync(message, userState);
                return NoContent();
            }
            else
                return BadRequest();
        }
    }
}
