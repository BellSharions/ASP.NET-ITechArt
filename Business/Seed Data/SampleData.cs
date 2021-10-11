using Business.Enums;
using DAL;
using DAL.Entities;
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
        public async void SeedProducts()
        {
            var SkyrimProduct = new Product("Skyrim", AvailablePlatforms.PC, "11.11.2011", 90);
            var UltrakillProduct = new Product("Ultrakill", AvailablePlatforms.PC, "03.09.2020", 100);
            var MinecraftProduct = new Product("Minecraft", AvailablePlatforms.XBOX, "18.11.2011", 95);
            var FalloutProduct = new Product("Skyrim", AvailablePlatforms.PC, "07.02.1997", 91);
            var GarrysModProduct = new Product("Garrys Mod", AvailablePlatforms.PC, "29.11.2006", 98);
            var HalfLifeProduct = new Product("HalfLife", AvailablePlatforms.PC, "19.11.1998", 80);
            var SekiroProduct = new Product("Sekiro: Shadows Die Twice", AvailablePlatforms.PlayStation, "22.03.2019", 76);
            var UntilDawnProduct = new Product("Until Dawn", AvailablePlatforms.PlayStation, "25.09.2015", 75);
            if (!_context.Products.Any(r => r.Id == 1))
            {
                await _context.Products.AddAsync(SkyrimProduct);
                await _context.Products.AddAsync(UltrakillProduct);
                await _context.Products.AddAsync(MinecraftProduct);
                await _context.Products.AddAsync(FalloutProduct);
                await _context.Products.AddAsync(GarrysModProduct);
                await _context.Products.AddAsync(HalfLifeProduct);
                await _context.Products.AddAsync(SekiroProduct);
                await _context.Products.AddAsync(UntilDawnProduct);
                await _context.SaveChangesAsync();
            }
        }

    }
}
