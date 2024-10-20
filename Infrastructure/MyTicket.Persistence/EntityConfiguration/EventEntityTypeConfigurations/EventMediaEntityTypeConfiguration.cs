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

        builder.Property(x => x.EventId)
         .IsRequired()
         .HasColumnName("event_id");

        builder.HasMany(x => x.Medias)
               .WithOne(x => x.EventMedia)
               .HasForeignKey(x => x.EventMediaId)
               .IsRequired();
    }
}