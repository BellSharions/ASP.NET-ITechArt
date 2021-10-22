using Business.Enums;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .Property(b => b.Id)
                .HasColumnName("ProductId")
                .IsRequired();

            builder.Property(b => b.Name).IsRequired();

            builder
                .Property(b => b.Platform)
                .IsRequired();

            builder
                .Property(b => b.DateCreated)
                .IsRequired();

            builder
                .Property(b => b.TotalRating)
                .IsRequired();
            builder.HasIndex(b => b.Name);
            builder.HasIndex(b => b.Platform);
            builder.HasIndex(b => b.TotalRating);
            builder.HasIndex(b => b.DateCreated);
            builder.HasData(
                new Product{Id = 1, Name = "Ultrakill", Platform = AvailablePlatforms.PC, DateCreated = "03.09.2020", TotalRating = 100, Count = "10", Genre = Enums.AvailableGenres.FPS, Price = "1000", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 2, Name = "Bloodborne", Platform = AvailablePlatforms.PlayStation, DateCreated = "24.04.2015", TotalRating = 84, Count = "15", Genre = Enums.AvailableGenres.RPG, Price = "1250", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 3, Name = "Minecraft", Platform = AvailablePlatforms.XBOX, DateCreated = "18.11.2011", TotalRating = 95, Count = "14", Genre = Enums.AvailableGenres.Sandbox, Price = "1520", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 4, Name = "Garrys Mod", Platform = AvailablePlatforms.PC, DateCreated = "29.11.2006", TotalRating = 98, Count = "17", Genre = Enums.AvailableGenres.Sandbox, Price = "1430", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 5, Name = "Animal Crossing", Platform = AvailablePlatforms.Switch, DateCreated = "20.03.2020", TotalRating = 75, Count = "25", Genre = Enums.AvailableGenres.FPS, Price = "1654", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 6, Name = "HalfLife", Platform = AvailablePlatforms.PC, DateCreated = "19.11.1998", TotalRating = 80, Count = "43", Genre = Enums.AvailableGenres.FPS, Price = "1352", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 7, Name = "Sekiro: Shadows Die Twice", Platform = AvailablePlatforms.PlayStation, DateCreated = "22.03.2019", TotalRating = 76, Count = "65", Genre = Enums.AvailableGenres.RPG, Price = "1532", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 8, Name = "Until Dawn", Platform = AvailablePlatforms.PlayStation, DateCreated = "25.09.2015", TotalRating = 75, Count = "15", Genre = Enums.AvailableGenres.Survival, Price = "1627", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 9, Name = "Portal 2", Platform = AvailablePlatforms.PC, DateCreated = "18.04.2011", TotalRating = 99, Count = "43", Genre = Enums.AvailableGenres.RPG, Price = "1243", Rating = Enums.AgeRating.PEGI16 },
                new Product{Id = 10, Name = "Skyrim", Platform = AvailablePlatforms.PC, DateCreated = "11.11.2011", TotalRating = 90, Count = "26", Genre = Enums.AvailableGenres.RPG, Price = "1234", Rating = Enums.AgeRating.PEGI16 }
            );
        }
    }
}
