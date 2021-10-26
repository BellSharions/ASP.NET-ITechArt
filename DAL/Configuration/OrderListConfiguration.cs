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
    public class OrderListConfiguration : IEntityTypeConfiguration<OrderList>
    {
        public void Configure(EntityTypeBuilder<OrderList> builder)
        {

            builder.HasKey(b => new {b.OrderId, b.ProductId});

            builder
                .HasOne(t => t.Product)
                .WithMany(t => t.OrderList)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(t => t.Order)
                .WithMany(t => t.OrderList)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            builder
                .Property(b => b.Amount)
                .IsRequired();

            builder.HasIndex(b => b.OrderId);
            builder.HasIndex(b => b.ProductId);

        }
    }
}
