using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Favourites;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Persistence.EntityConfiguration.WishListEntityTypeConfigurations;
public class WishListEntityTypeConfiguration : IEntityTypeConfiguration<WishList>
{
    public void Configure(EntityTypeBuilder<WishList> builder)
    {
        builder.ToTable("wish_lists");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
               .HasColumnName("id");

        builder.Property(w => w.UserId)
               .IsRequired()
               .HasColumnName("user_id");

        // Relation between WishList and WishListEvent (one-to-many)
        builder.HasMany(w => w.WishListEvents)
               .WithOne(we => we.WishList)
               .HasForeignKey(we => we.WishListId)
               .OnDelete(DeleteBehavior.Cascade);

        // Define the one-to-one relationship with User
        builder.HasOne(b => b.User)
               .WithOne(u => u.WishList)
               .HasForeignKey<User>(x => x.WishListId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}
