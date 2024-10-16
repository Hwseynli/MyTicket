using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Events;

namespace MyTicket.Persistence.EntityConfiguration.EventEntityTypeConfigurations;
public class RatingEntityTypeConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("event_ratings");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Id)
               .HasColumnName("id");

        // Relationships
        builder.HasOne(x => x.User)
               .WithMany(u => u.Ratings)
               .HasForeignKey(x => x.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Event)
               .WithMany(e => e.Ratings)
               .HasForeignKey(x => x.EventId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        // Properties
        builder.Property(x => x.RatingValue)
               .IsRequired()
               .HasColumnName("rating_value")
               .HasConversion<int>(); // Enum dəyərinin int kimi saxlanılması

        builder.Property(x => x.RatedAt)
               .IsRequired()
               .HasColumnName("rated_at");
    }
}
