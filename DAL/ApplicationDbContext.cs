using DAL.Configuration;
using DAL.Entities;
using DAL.Entities.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int> 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductRating> ProductRating { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderList> OrderList { get; set; }
    }
}
