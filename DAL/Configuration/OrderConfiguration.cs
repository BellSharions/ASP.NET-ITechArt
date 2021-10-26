using DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configuration
{
    public class OrderConfiguration
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.HasKey(b => b.OrderId);

            builder
                .Property(b => b.OrderId)
                .IsRequired();

            builder
                .HasOne(t => t.Product)
                .WithMany(t => t.OrdersList)
                .HasForeignKey(p => p.ProductId)
                .IsRequired();

            builder
               .HasOne(t => t.User)
               .WithMany(u => u.OrdersList)
               .HasForeignKey(b => b.UserId)
                .IsRequired();

            builder
                .Property(b => b.Amount)
                .IsRequired();
            builder
                .Property(b => b.CreationDate)
                .IsRequired();
            builder
                .Property(b => b.Status)
                .IsRequired();

        }
    }
}
