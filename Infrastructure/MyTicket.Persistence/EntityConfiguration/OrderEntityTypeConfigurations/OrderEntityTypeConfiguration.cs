using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Orders;

namespace MyTicket.Persistence.EntityConfiguration.OrderEntityTypeConfigurations;
public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
               .HasColumnName("id");

        builder.Property(o => o.OrderCode)
               .IsRequired()
               .HasColumnName("order_code");

        builder.Property(o => o.UserId)
               .IsRequired()
               .HasColumnName("user_id");

        builder.Property(o => o.OrderDate)
               .IsRequired()
               .HasColumnName("order_date");

        builder.Property(o => o.IsPaid)
               .HasDefaultValue(false)
               .HasColumnName("is_paid");

        builder.Property(o => o.TotalAmount)
               .IsRequired()
               .HasColumnType("decimal(18,2)")
               .HasColumnName("total_amount");

        builder.Property(o => o.PromoCodeId)
               .HasColumnName("promo_code_id");

        // Relationships

        builder.HasMany(o => o.Tickets)
               .WithOne(t => t.Order)
               .HasForeignKey(t => t.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

