using ASP_NET.Models;
using AutoMapper;
using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class UserRepository : GenericRepository<ApplicationDbContext, User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext dbContext, UserManager<User> userManager, IMapper mapper) : base(dbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> UpdatePasswordAsync(User info, string token, string passwordHash)
        {
            var userForUpdate = GetCurrentUser(info.Email);
            var result = await _userManager.ResetPasswordAsync(info, token, passwordHash);
            if (result.Succeeded)
            {
                //await UpdateItemAsync(userForUpdate);
                _dbContext.Users.Update(userForUpdate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> UpdateUserInfoAsync(int id, UserModel user)
        {
            var foundUser = await _userManager.FindByIdAsync(id.ToString());
            foundUser.UserName = user.UserName;
            foundUser.Email = user.Email;
            foundUser.PhoneNumber = user.PhoneNumber;
            foundUser.adressDelivery = user.AdressDelivery;
            _dbContext.Update(foundUser);
            if(await _dbContext.SaveChangesAsync() == 0)
                return null;
            return foundUser;
        }
        public async Task<User> GetUserByIdAsync(int id) => await _userManager.FindByIdAsync(id.ToString());
        public User GetCurrentUser(string email) => _dbContext.Users.FirstOrDefault(u => u.Email == email);
    }
}
