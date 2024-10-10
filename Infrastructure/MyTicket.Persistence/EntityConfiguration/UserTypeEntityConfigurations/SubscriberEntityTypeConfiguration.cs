using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Persistence.EntityConfiguration.UserTypeEntityConfigurations;
public class SubscriberEntityTypeConfiguration : IEntityTypeConfiguration<Subscriber>
{
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable("subscribers");
        builder.HasKey(t => t.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id");

        builder.Property(x => x.Email)
               .IsRequired()
               .HasColumnName("email");

        builder.HasIndex(t => t.Email)
           .IsUnique();

        builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasColumnName("phone_number");

        builder.HasIndex(t => t.PhoneNumber)
           .IsUnique();

        builder.Property(x => x.CreatedDateTime)
             .HasColumnName("created_date_time");
    }
}
