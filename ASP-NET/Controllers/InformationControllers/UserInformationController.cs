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
        [Route("user")]
        [HttpPut]
        public async Task<IActionResult> ChangeUserInfo([FromBody] UserModel info)
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
        [Route("user")]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _userManager.FindByIdAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }

        [Authorize(Roles = "User, Admin")]
        [Route("user/password")]
        [HttpPatch]
        public async Task<IActionResult> ChangeUserPassword([FromBody] JsonPatchDocument<User> patchDoc)
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
