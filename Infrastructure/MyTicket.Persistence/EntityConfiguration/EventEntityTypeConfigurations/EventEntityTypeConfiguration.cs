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
        builder.HasKey(t => t.Id);

        // Properties
        builder.Property(x => x.Id)
               .HasColumnName("id");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnName("title");

        builder.Property(x => x.MinPrice)
           .IsRequired()
           .HasColumnName("min_price");

        builder.Property(x => x.Description)
            .HasColumnName("description");

        builder.Property(x => x.StartTime)
            .IsRequired()
            .HasColumnName("start_date_time");

        builder.Property(x => x.EndTime)
           .HasColumnName("end_date_time");

        builder.Property(x => x.Language)
               .IsRequired()
               .HasColumnName("language")
               .HasConversion<string>(); // Enum dəyərinin string kimi saxlanılması

        builder.Property(x => x.MinAge)
               .IsRequired()
               .HasColumnName("min_age");

        builder.Property(x => x.IsDeleted)
           .HasColumnName("is_deleted");

        builder.Property(x => x.PlaceHallId)
            .IsRequired()
            .HasColumnName("place_hall_id");

        builder.Property(x => x.SubCategoryId)
           .IsRequired()
           .HasColumnName("sub_category_id");

        builder.Property(x => x.AverageRating)
           .IsRequired()
           .HasColumnName("average_rating");

        builder.HasMany(ph => ph.EventMedias)
            .WithOne(s => s.Event)
            .HasForeignKey(s => s.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Ratings)
         .WithOne(uer => uer.Event)
         .HasForeignKey(uer => uer.EventId)
         .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Tickets)
       .WithOne(uer => uer.Event)
       .HasForeignKey(uer => uer.EventId)
       .OnDelete(DeleteBehavior.Cascade);

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

