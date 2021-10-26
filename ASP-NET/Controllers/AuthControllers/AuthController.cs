using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Business.Interfaces;
using Business.Models;

namespace ASP_NET.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("sign-in")]
        [SwaggerOperation(
            Summary = "Signs in specified user",
            Description = "Requires email and pasword in json format",
            OperationId = "SignIn",
            Tags = new[] { "Authorization", "User" })]
        [SwaggerResponse(200, "User was signed in")]
        [SwaggerResponse(400, "User was not signed in due to invalid information")]
        public async Task<IActionResult> SignIn([FromBody, SwaggerParameter("Information containing email and password", Required = true)] CreateUserModel info)
        {
            var result = await _userService.SigninAsync(info);
            if (result.Type.ToString() == "BadRequest")
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("sign-up")]
        [SwaggerOperation(
            Summary = "Registeres new user",
            Description = "Requires email and pasword in json format",
            OperationId = "SignUp",
            Tags = new[] { "Authorization", "User" })]
        [SwaggerResponse(201, "User was created")]
        [SwaggerResponse(400, "User was not created due to invalid information")]
        public async Task<IActionResult> SignUp([FromBody, SwaggerParameter("Information containing email and password", Required = true)] CreateUserModel info)
        {
            var result = await _userService.RegisterAsync(info);
            if(result.Type.ToString() == "Created")
                return Created("api/auth/sign-up", null);
            return BadRequest(result.Message);
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
            var isConfirmed = await _userService.ConfirmEmailAsync(id, token);
            if(isConfirmed.Type.ToString() == "NoContent")
                return NoContent();
            return BadRequest(isConfirmed.Message);
        }
    }
}
