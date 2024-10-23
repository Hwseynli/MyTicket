using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Events;

namespace MyTicket.Persistence.EntityConfiguration.EventEntityTypeConfigurations;
public class TicketEntityTypeConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");

        // Primary Key
        builder.HasKey(t => t.Id);

        // Properties
        builder.Property(x => x.Id)
               .HasColumnName("id");

        builder.Property(x => x.UniqueCode)
         .IsRequired()
         .HasColumnName("unique_code");

        builder.Property(x => x.EventId)
          .IsRequired()
          .HasColumnName("event_id");

        builder.Property(x => x.SeatId)
            .IsRequired()
            .HasColumnName("seat_id");

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.Price)
         .IsRequired()
         .HasColumnName("price");

        builder.Property(x => x.IsSold)
         .HasDefaultValue(false)
         .HasColumnName("is_sold");

        builder.Property(x => x.IsReserved)
        .HasDefaultValue(false)
        .HasColumnName("is_reserved");

        // Audit Fields
        builder.Property(x => x.CreatedById)
            .HasColumnName("created_by_id");

        builder.Property(x => x.RecordDateTime)
    .HasColumnName("record_date_time");

        builder.Property(x => x.UpdateById)
            .HasColumnName("update_by_id");

        builder.Property(x => x.LastUpdateDateTime)
    .HasColumnName("last_update_date_time");
    }
}

