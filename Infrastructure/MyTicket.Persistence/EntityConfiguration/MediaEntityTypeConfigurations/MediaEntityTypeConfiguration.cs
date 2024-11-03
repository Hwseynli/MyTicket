using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Medias;

namespace MyTicket.Persistence.EntityConfiguration.MediaEntityTypeConfigurations;
public class MediaEntityTypeConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.ToTable("medias");

        builder.HasKey(t => t.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id");

        builder.Property(x => x.Name)
              .IsRequired()
              .HasColumnName("name");

        builder.Property(x => x.Path)
             .IsRequired()
             .HasColumnName("path");

        builder.Property(x => x.Others)
            .HasColumnName("others");

        builder.Property(x => x.IsMain)
            .IsRequired()
            .HasColumnName("is_main");

        builder.Property(x => x.MediaType)
            .IsRequired()
            .HasColumnName("media_type");

        builder.Property(x => x.EventMediaId)
            .IsRequired()
            .HasColumnName("event_media_id");

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

