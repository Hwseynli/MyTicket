using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Places;

namespace MyTicket.Persistence.EntityConfiguration.PlaceEntityTypeConfigurations;
public class SeatEntityTypeConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("seats");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
               .HasColumnName("id");

        builder.Property(s => s.RowNumber)
               .IsRequired()
               .HasColumnName("row_number");

        builder.Property(s => s.SeatNumber)
               .IsRequired()
               .HasColumnName("seat_number");

        builder.Property(s => s.SeatType)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(50)
               .HasColumnName("seat_type");

        builder.Property(s => s.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)")
               .HasColumnName("price");

        builder.Property(s => s.PlaceHallId)
               .HasColumnName("place_hall_id");

        builder.Property(x => x.CreatedById)
           .HasColumnName("created_by_id");

        builder.Property(x => x.LastUpdateDateTime)
            .HasColumnName("last_update_date_time");

        builder.Property(x => x.UpdateById)
           .HasColumnName("update_by_id");

        builder.Property(x => x.RecordDateTime)
           .HasColumnName("record_date_time");
    }
}
