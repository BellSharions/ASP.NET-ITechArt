using Business.DTO;
using Business.Interfaces;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Repositories
{
    public class UserRepository : GenericRepository<ApplicationDbContext, User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        public async Task<bool> UpdatePasswordAsync(ChangePasswordUserDto user)
        {
            var userForUpdate = await GetUser(user.Id);
            var result = await _userManager.ChangePasswordAsync(userForUpdate, user.OldPassword, user.NewPassword);
            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<User> UpdateUserInfoAsync(User user)
        {
            _dbContext.Update(user);
            if(await _dbContext.SaveChangesAsync() == 0)
                return null;
            return user;
        }
        public async Task<User> GetUser(int id) => await _userManager.FindByIdAsync(id.ToString());
        public async Task<User> GetUser(string email) => await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
