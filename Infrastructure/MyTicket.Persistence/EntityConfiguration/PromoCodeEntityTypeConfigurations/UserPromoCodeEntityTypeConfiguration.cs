using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.PromoCodes;

namespace MyTicket.Persistence.EntityConfiguration.PromoCodeEntityTypeConfigurations;
public class UserPromoCodeEntityTypeConfiguration : IEntityTypeConfiguration<UserPromoCode>
{
    public void Configure(EntityTypeBuilder<UserPromoCode> builder)
    {
        builder.ToTable("user_promo_codes");
        builder.HasKey(upc => upc.Id);

        builder.Property(upc => upc.Id)
            .HasColumnName("id");

        builder.Property(upc => upc.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(upc => upc.PromoCodeId)
            .IsRequired()
            .HasColumnName("promo_code_id");

        builder.Property(upc => upc.UsedDate)
            .IsRequired()
            .HasColumnName("used_date");
    }
}

