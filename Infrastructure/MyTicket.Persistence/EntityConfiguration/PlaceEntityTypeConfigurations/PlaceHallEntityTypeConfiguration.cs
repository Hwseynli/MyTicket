using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Places;

namespace MyTicket.Persistence.EntityConfiguration.PlaceEntityTypeConfigurations;
public class PlaceHallEntityTypeConfiguration : IEntityTypeConfiguration<PlaceHall>
{
    public void Configure(EntityTypeBuilder<PlaceHall> builder)
    {
        builder.ToTable("place_halls");
        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.Id)
               .HasColumnName("id");

        builder.Property(ph => ph.Name)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("name");

        builder.Property(ph => ph.PlaceId)
                .IsRequired()
               .HasColumnName("place_id");

        builder.Property(ph => ph.SeatCount)
                .IsRequired()
              .HasColumnName("seat_count");

        builder.Property(ph => ph.RowCount)
               .IsRequired()
             .HasColumnName("row_count");

        builder.HasMany(ph => ph.Seats)
               .WithOne(s => s.PlaceHall)
               .HasForeignKey(s => s.PlaceHallId)
               .OnDelete(DeleteBehavior.Cascade);
        // Relationships
        builder.HasMany(ph => ph.Events)
            .WithOne(e => e.PlaceHall)
            .HasForeignKey(e => e.PlaceHallId)
            .IsRequired();

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

