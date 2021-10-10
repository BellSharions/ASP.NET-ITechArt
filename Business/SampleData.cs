using DAL;
using DAL.Entities.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class SampleData
    {
        private ApplicationDbContext _context;

        public SampleData(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void SeedRoles()
        {
            var adminRole = new Role("Admin", "Administrator");
            var userRole = new Role("User", "User");
            if (!_context.Roles.Any(r => r.Name == "Admin") && !_context.Roles.Any(r => r.Name == "User"))
            {
                await _context.Roles.AddAsync(adminRole);
                await _context.Roles.AddAsync(userRole);
                await _context.SaveChangesAsync();
            }
        }

    }
}
