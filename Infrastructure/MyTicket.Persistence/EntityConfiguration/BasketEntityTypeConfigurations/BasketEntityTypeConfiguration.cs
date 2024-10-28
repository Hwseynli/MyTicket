using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Baskets;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Persistence.EntityConfiguration.BasketEntityTypeConfigurations;
public class BasketEntityTypeConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.ToTable("baskets");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
               .HasColumnName("id");

        builder.Property(w => w.UserId)
               .IsRequired()
               .HasColumnName("user_id");

        builder.HasMany(b => b.TicketsWithTime)
        .WithOne(t => t.Basket)
        .HasForeignKey(t => t.BasketId)
        .OnDelete(DeleteBehavior.Cascade);

        // Define the one-to-one relationship with User
        builder.HasOne(b => b.User)
               .WithOne(u => u.Basket)
               .HasForeignKey<User>(x => x.BasketId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
