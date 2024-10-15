using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Events;

namespace MyTicket.Persistence.EntityConfiguration.EventEntityTypeConfigurations;
public class EventMediaEntityTypeConfiguration : IEntityTypeConfiguration<EventMedia>
{
    public void Configure(EntityTypeBuilder<EventMedia> builder)
    {
        builder.ToTable("event_medias");

        // Primary Key
        builder.HasKey(t => t.Id);

        // Properties
        builder.Property(x => x.Id)
               .HasColumnName("id");

        builder.Property(x => x.MediaType)
            .IsRequired()
            .HasColumnName("media_type");

        builder.Property(x => x.Path)
            .IsRequired()
            .HasColumnName("url");

        builder.Property(x => x.IsMain)
            .IsRequired()
            .HasColumnName("is_main");

        builder.Property(x => x.EventId)
            .IsRequired()
            .HasColumnName("event_id");

        // Audit Fields
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
