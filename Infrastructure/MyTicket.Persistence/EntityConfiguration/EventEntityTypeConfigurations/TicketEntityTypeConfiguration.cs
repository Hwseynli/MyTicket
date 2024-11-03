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
        builder.Property(t => t.Id)
               .HasColumnName("id");

        builder.Property(t => t.UniqueCode)
         .IsRequired()
         .HasColumnName("unique_code");

        builder.Property(t => t.EventId)
          .IsRequired()
          .HasColumnName("event_id");

        builder.Property(t => t.SeatId)
            .IsRequired()
            .HasColumnName("seat_id");

        builder.Property(t => t.UserId)
            .HasColumnName("user_id");

        builder.Property(t => t.OrderId)
           .HasColumnName("order_id");

        builder.Property(t => t.Price)
         .IsRequired()
         .HasColumnName("price");

        builder.Property(t => t.IsSold)
         .HasDefaultValue(false)
         .HasColumnName("is_sold");

        builder.Property(t => t.IsReserved)
        .HasDefaultValue(false)
        .HasColumnName("is_reserved");

        // Audit Fields
        builder.Property(t => t.CreatedById)
            .HasColumnName("created_by_id");

        builder.Property(t => t.RecordDateTime)
    .HasColumnName("record_date_time");

        builder.Property(t => t.UpdateById)
            .HasColumnName("update_by_id");

        builder.Property(t => t.LastUpdateDateTime)
    .HasColumnName("last_update_date_time");
    }
}

