using ASP_NET.Models;
using AutoMapper;
using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Entities.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET.Controllers.InformationControllers
{

    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class UserInformationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserInformationController(UserManager<User> userManager, IUserService userService, RoleManager<Role> roleManager, IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("user")]
        [SwaggerOperation(
            Summary = "Change user information",
            Description = "Searches specified user and changes information taken from body",
            OperationId = "ChangeUserInfo",
            Tags = new[] { "Information", "User" })]
        [SwaggerResponse(200, "User information was updated")]
        [SwaggerResponse(400, "User information was not updated")]
        public async Task<IActionResult> ChangeUserInfo([FromBody, SwaggerParameter("Modified user information", Required = true)] UserModel info)
        {
            var result = await _userService.UpdateUserInfoAsync(int.Parse(HttpContext.User.Claims.FirstOrDefault().Value), info);
            if(result == null)
                return BadRequest();
            return Ok();

        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("user")]
        [SwaggerOperation(
            Summary = "Get current user",
            Description = "Searches and gets user information that is currently authorized",
            OperationId = "GetUser",
            Tags = new[] { "Information", "User" })]
        [SwaggerResponse(200, "Returned current user", typeof(User))]
        public async Task<IActionResult> GetUser() => Ok(await _userService.FindUserByIdAsync(int.Parse(HttpContext.User.Claims.FirstOrDefault().Value)));

        [Authorize(Roles = "User, Admin")]
        [HttpPatch("user/password")]
        [SwaggerOperation(
            Summary = "Change user password",
            Description = "Searches and changes user password using PATCH method",
            OperationId = "ChangeUserPassword",
            Tags = new[] { "Information", "User" })]
        [SwaggerResponse(204, "User password was changed", typeof(User))]
        [SwaggerResponse(400, "Patch method was incorrect")]
        public async Task<IActionResult> ChangeUserPassword([FromBody, SwaggerParameter("PATCH method to change password", Required = true)] JsonPatchDocument<User> patchDoc)
        {
            if (patchDoc == null && !ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.ChangePasswordAsync(int.Parse(HttpContext.User.Claims.FirstOrDefault().Value), patchDoc);
            if (result == 400)
                return BadRequest(ModelState);
            return Created("user/password", null);
        }

        protected IActionResult Index() => View();
    }
}
