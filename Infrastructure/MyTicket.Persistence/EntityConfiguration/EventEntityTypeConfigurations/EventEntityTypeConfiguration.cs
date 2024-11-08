using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Events;

namespace MyTicket.Persistence.EntityConfiguration.EventEntityTypeConfigurations;
public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");

        // Primary Key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Id)
               .HasColumnName("id");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasColumnName("title");

        builder.HasIndex(e => e.Title)
            .IsUnique();

        builder.Property(e => e.MinPrice)
           .IsRequired()
           .HasColumnName("min_price");

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.StartTime)
            .IsRequired()
            .HasColumnName("start_date_time");

        builder.Property(e => e.EndTime)
           .HasColumnName("end_date_time");

        builder.Property(e => e.Language)
               .IsRequired()
               .HasColumnName("language")
               .HasConversion<string>(); // Enum dəyərinin string kimi saxlanılması

        builder.Property(e => e.MinAge)
               .IsRequired()
               .HasColumnName("min_age");

        builder.Property(e => e.IsDeleted)
           .HasColumnName("is_deleted");

        builder.Property(e => e.PlaceHallId)
            .IsRequired()
            .HasColumnName("place_hall_id");

        builder.Property(e => e.AverageRating)
           .IsRequired()
           .HasColumnName("average_rating");

        builder.HasMany(e => e.EventMedias)
            .WithOne(em => em.Event)
            .HasForeignKey(em => em.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Ratings)
         .WithOne(r => r.Event)
         .HasForeignKey(r => r.EventId)
         .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Tickets)
       .WithOne(t => t.Event)
       .HasForeignKey(t => t.EventId)
       .OnDelete(DeleteBehavior.Cascade);

        // Relation between WishListEvent and Event
        builder.HasMany(e => e.WishListEvents)
               .WithOne(we => we.Event)
               .HasForeignKey(we => we.EventId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.SubCategories)
               .WithMany(sc => sc.Events)
               .UsingEntity(j => j.ToTable("event_subcategories"));

        // Audit Fields
        builder.Property(e => e.CreatedById)
            .HasColumnName("created_by_id");

        builder.Property(e => e.LastUpdateDateTime)
            .HasColumnName("last_update_date_time");

        builder.Property(e => e.UpdateById)
            .HasColumnName("update_by_id");

        builder.Property(e => e.RecordDateTime)
            .HasColumnName("record_date_time");
    }
}

