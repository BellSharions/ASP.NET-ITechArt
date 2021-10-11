using ASP_NET.Models;
using AutoMapper;
using Business;
using Business.Interfaces;
using Business.Repositories;
using DAL;
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
        private readonly IRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserInformationController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userRepository = new UserRepository(context);
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
            var foundUser = await _userRepository.Get(int.Parse(HttpContext.User.Claims.FirstOrDefault().Value));
            foundUser.UserName = info.UserName;
            foundUser.Email = info.Email;
            foundUser.PhoneNumber = info.PhoneNumber;
            foundUser.adressDelivery = info.AdressDelivery;
            _userRepository.Update(foundUser);
            if(await _userRepository.Save() != 0)
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
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _userRepository.Get(int.Parse(HttpContext.User.Claims.FirstOrDefault().Value));
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
            if (patchDoc == null) //change to avoid if nesting
                return BadRequest(ModelState);
            var user = await _userRepository.Get(int.Parse(HttpContext.User.Claims.FirstOrDefault().Value));
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newUser = user;
            patchDoc.ApplyTo(newUser, ModelState);

            if (!ModelState.IsValid && !(await _userManager.ResetPasswordAsync(user, token, newUser.PasswordHash)).Succeeded)
                return BadRequest(ModelState);
            return Created("user/password", user);
        }

        protected IActionResult Index()
        {
            return View();
        }
    }
}
