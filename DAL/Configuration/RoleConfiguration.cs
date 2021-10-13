using DAL.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role{Id = 1, Name = "Admin",NormalizedName = "Administrator" },
                new Role{Id = 2, Name = "User", NormalizedName = "User" });
        }
    }
}
