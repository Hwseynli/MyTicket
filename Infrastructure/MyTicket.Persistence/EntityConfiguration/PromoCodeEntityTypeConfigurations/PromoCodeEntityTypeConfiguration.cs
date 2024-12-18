﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.PromoCodes;

namespace MyTicket.Persistence.EntityConfiguration.PromoCodeEntityTypeConfigurations;
public class PromoCodeEntityTypeConfiguration : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.ToTable("promo_codes");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.UniqueCode)
            .IsRequired()
            .HasColumnName("unique_code")
            .HasMaxLength(50);

        builder.HasIndex(p => p.UniqueCode)
            .IsUnique();

        builder.Property(p => p.DiscountAmount)
            .IsRequired()
            .HasColumnName("discount_amount")
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.DiscountType)
            .IsRequired()
            .HasColumnName("discount_type");

        builder.Property(p => p.ExpirationDate)
            .IsRequired()
            .HasColumnName("expiration_date");

        builder.Property(p => p.DeletedDate)
            .HasColumnName("deleted_date");

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder.Property(p => p.UsageLimit)
            .IsRequired()
            .HasColumnName("usage_limit");

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true)
            .IsRequired()
            .HasColumnName("is_active");

        builder.HasMany(p => p.UserPromoCodes)
            .WithOne(upc => upc.PromoCode)
            .HasForeignKey(upc => upc.PromoCodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Orders)
        .WithOne(o => o.PromoCode)
        .HasForeignKey(o => o.PromoCodeId)
        .OnDelete(DeleteBehavior.SetNull);

    }
}
