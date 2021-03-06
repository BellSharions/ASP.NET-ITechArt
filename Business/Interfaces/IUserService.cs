using Business.DTO;
using Business.Models;
using DAL.Entities;
using DAL.Entities.Models;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService 
    {
        Task<ServiceResult> RegisterAsync(CreateUserModel info);
        Task<ServiceResult> ConfirmEmailAsync(int id, string token);
        Task<ServiceResult> SigninAsync(CreateUserModel info);
        Task<ServiceResult> ChangePasswordAsync(ChangePasswordUserDto user);
        Task<User> FindUserByIdAsync(int id);
        Task<UserInfoDto> FindUserInfoByIdAsync(int id);
        Task<User> UpdateUserInfoAsync(int id, ChangeUserInfoDto user);
    }
}
