using Business.DTO;
using DAL.Entities;
using DAL.Interfaces;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUser(int id);
        Task<User> GetUser(string email);
        Task<User> UpdateUserInfoAsync(User user);
        Task<bool> UpdatePasswordAsync(ChangePasswordUserDto user);
    }
}
