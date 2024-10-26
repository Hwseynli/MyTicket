using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Favourites;

namespace MyTicket.Persistence.EntityConfiguration.WishListEntityTypeConfigurations;
public class WishListEventEntityTypeConfiguration : IEntityTypeConfiguration<WishListEvent>
{
    public void Configure(EntityTypeBuilder<WishListEvent> builder)
    {
        builder.ToTable("wish_list_events");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
               .HasColumnName("id");

        builder.Property(we => we.WishListId)
               .HasColumnName("wish_list_id");

        builder.Property(we => we.EventId)
               .HasColumnName("event_id");

    }
}