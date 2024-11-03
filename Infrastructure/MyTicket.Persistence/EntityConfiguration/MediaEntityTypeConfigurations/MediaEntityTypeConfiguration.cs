using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Medias;

namespace MyTicket.Persistence.EntityConfiguration.MediaEntityTypeConfigurations;
public class MediaEntityTypeConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.ToTable("medias");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
               .HasColumnName("id");

        builder.Property(m => m.Name)
              .IsRequired()
              .HasColumnName("name");

        builder.Property(m => m.Path)
             .IsRequired()
             .HasColumnName("path");

        builder.Property(m => m.Others)
            .HasColumnName("others");

        builder.Property(m => m.IsMain)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_main");

        builder.Property(m => m.MediaType)
            .IsRequired()
            .HasColumnName("media_type");

        builder.Property(m => m.EventMediaId)
            .IsRequired()
            .HasColumnName("event_media_id");

        builder.Property(m => m.CreatedById)
             .HasColumnName("created_by_id");

        builder.Property(m => m.LastUpdateDateTime)
            .HasColumnName("last_update_date_time");

        builder.Property(m => m.UpdateById)
           .HasColumnName("update_by_id");

        builder.Property(m => m.RecordDateTime)
           .HasColumnName("record_date_time");
    }
}

