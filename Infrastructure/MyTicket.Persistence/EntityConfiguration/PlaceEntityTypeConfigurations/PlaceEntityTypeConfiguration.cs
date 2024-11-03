using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Places;

namespace MyTicket.Persistence.EntityConfiguration.PlaceEntityTypeConfigurations;
public class PlaceEntityTypeConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.ToTable("places");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .HasColumnName("id");

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("name");

        builder.HasIndex(p => p.Name)
               .IsUnique();

        builder.Property(p => p.Address)
               .IsRequired()
               .HasMaxLength(250)
               .HasColumnName("address");

        builder.HasMany(p => p.PlaceHalls)
               .WithOne(ph => ph.Place)
               .HasForeignKey(ph => ph.PlaceId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.CreatedById)
            .HasColumnName("created_by_id");

        builder.Property(p => p.LastUpdateDateTime)
            .HasColumnName("last_update_date_time");

        builder.Property(p => p.UpdateById)
           .HasColumnName("update_by_id");

        builder.Property(p => p.RecordDateTime)
           .HasColumnName("record_date_time");
    }
}

