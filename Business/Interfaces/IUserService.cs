using ASP_NET.Models;
using DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService 
    {
        Task<int> RegisterAsync(UserModel info);
        Task<int> ConfirmEmailAsync(int id, string token);
        Task<int> SigninAsync(UserModel info);
        Task<int> ChangePasswordAsync(int id, JsonPatchDocument<User> patchDoc);
        Task<User> FindUserByIdAsync(int id);
        Task<User> UpdateUserInfoAsync(int id, UserModel user);
    }
}
