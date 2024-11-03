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
        builder.HasKey(em => em.Id);

        // Properties
        builder.Property(em => em.Id)
               .HasColumnName("id");

        builder.Property(em => em.EventId)
         .IsRequired()
         .HasColumnName("event_id");

        // Audit Fields
        builder.Property(em => em.CreatedById)
            .HasColumnName("created_by_id");

        builder.Property(em => em.LastUpdateDateTime)
            .HasColumnName("last_update_date_time");

        builder.Property(em => em.UpdateById)
            .HasColumnName("update_by_id");

        builder.Property(em => em.RecordDateTime)
            .HasColumnName("record_date_time");

        builder.HasMany(em => em.Medias)
               .WithOne(em => em.EventMedia)
               .HasForeignKey(em => em.EventMediaId)
               .IsRequired();
    }
}