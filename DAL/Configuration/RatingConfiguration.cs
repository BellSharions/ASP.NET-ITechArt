using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    public class RatingConfiguration : IEntityTypeConfiguration<ProductRating>
    {
        public void Configure(EntityTypeBuilder<ProductRating> builder)
        {

            builder.HasKey(b => b.RatingId);

            builder
                .Property(b => b.RatingId)
                .IsRequired();

            builder
                .HasOne(t => t.Product)
                .WithMany(t => t.Ratings)
                .HasForeignKey(p => p.ProductId)
                .IsRequired();

            builder
               .HasOne(t => t.User)
               .WithMany(u => u.Ratings)
               .HasForeignKey(b => b.UserId)
                .IsRequired();

            builder
                .Property(b => b.Rating)
                .IsRequired();

        }
    }
}
