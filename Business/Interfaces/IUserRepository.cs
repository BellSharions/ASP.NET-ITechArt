using ASP_NET.Models;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByIdAsync(int id);
        User GetCurrentUser(string email);
        Task<User> UpdateUserInfoAsync(int id, UserModel user);
        Task<bool> UpdatePasswordAsync(User info, string token, string passwordHash);
    }
}
