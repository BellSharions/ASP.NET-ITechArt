using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using System;
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
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _iHttpContextAccessor;

        public UserInformationController(IUserService userService, IMemoryCache memoryCache, IHttpContextAccessor iHttpContextAccessor)
        {
            _userService = userService;
            _memoryCache = memoryCache;
            _iHttpContextAccessor = iHttpContextAccessor;
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
        public async Task<IActionResult> ChangeUserInfo([FromBody, SwaggerParameter("Modified user information", Required = true)] ChangeUserInfoDto info)
        {
            var userId = _iHttpContextAccessor.HttpContext.User.Claims.First().Value;
            if (userId == "")
                return BadRequest();
            var result = await _userService.UpdateUserInfoAsync(int.Parse(userId), info);
            if(result == null)
                return BadRequest();
            _memoryCache.Remove(int.Parse(_iHttpContextAccessor.HttpContext.User.Claims.First().Value));
            return Ok();

        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("user")]
        [SwaggerOperation(
            Summary = "Get current user",
            Description = "Searches and gets user information that is currently authorized",
            OperationId = "GetUser",
            Tags = new[] { "Information", "User" })]
        [SwaggerResponse(200, "Returned current user", typeof(UserInfoDto))]
        public async Task<IActionResult> GetUser() 
        {
            var userId = _iHttpContextAccessor.HttpContext.User.Claims.First().Value;
            if (userId == "")
                return BadRequest();
            if (!_memoryCache.TryGetValue(int.Parse(userId), out UserInfoDto user))
            {
                user = await _userService.FindUserInfoByIdAsync(int.Parse(userId));
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromDays(5)
                };
                _memoryCache.Set(int.Parse(userId), user, cacheExpiryOptions);
            }
            return Ok(user);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPatch("user/password")]
        [SwaggerOperation(
            Summary = "Change user password",
            Description = "Searches and changes user password using PATCH method",
            OperationId = "ChangeUserPassword",
            Tags = new[] { "Information", "User" })]
        [SwaggerResponse(204, "User password was changed")]
        [SwaggerResponse(400, "Patch method was incorrect")]
        public async Task<IActionResult> ChangeUserPassword([FromBody, SwaggerParameter("PATCH method to change password", Required = true)] JsonPatchDocument<ChangePasswordUserDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest(ModelState);
            var user = new ChangePasswordUserDto();
            patchDoc.ApplyTo(user);
            var result = await _userService.ChangePasswordAsync(user);
            if (result.Type.ToString() == "0")
                return BadRequest(ModelState);
            _memoryCache.Remove(int.Parse(_iHttpContextAccessor.HttpContext.User.Claims.First().Value));
            return Created("user/password", result.Message);
        }
    }
}
