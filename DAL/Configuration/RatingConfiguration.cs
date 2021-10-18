using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configuration
{
    public class RatingConfiguration : IEntityTypeConfiguration<ProductRating>
    {
        public void Configure(EntityTypeBuilder<ProductRating> builder)
        {

            builder.HasKey(b => new { b.ProductId, b.UserId });

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

            builder.HasIndex(b => b.Rating);
        }
    }
}
