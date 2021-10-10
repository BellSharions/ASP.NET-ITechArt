using ASP_NET.Models;
using AutoMapper;
using Business;
using DAL.Entities;
using DAL.Entities.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP_NET.Controllers.InformationControllers
{

    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class UserInformationController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserInformationController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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
            var foundUser = _userManager.FindByIdAsync(HttpContext.User.Claims.FirstOrDefault().Value).Result;
            foundUser.UserName = info.UserName;
            foundUser.Email = info.Email;
            foundUser.PhoneNumber = info.PhoneNumber;
            foundUser.adressDelivery = info.AdressDelivery;
            //foundUser = _mapper.Map<User>(info);
            if ((await _userManager.UpdateAsync(foundUser)).Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("user")]
        [SwaggerOperation(
            Summary = "Get current user",
            Description = "Searches and gets user information that is currently authorized",
            OperationId = "GetUser",
            Tags = new[] { "Information", "User" })]
        [SwaggerResponse(200, "Returned current user", typeof(User))]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _userManager.FindByIdAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }

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
            if (patchDoc != null)
            {
                var user = await _userManager.FindByIdAsync(HttpContext.User.Claims.FirstOrDefault().Value);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var newUser = user;
                patchDoc.ApplyTo(newUser, ModelState);

                if (ModelState.IsValid && (await _userManager.ResetPasswordAsync(user, token, newUser.PasswordHash)).Succeeded)
                {
                    return Created("user/password", user);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        protected IActionResult Index()
        {
            return View();
        }
    }
}
