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

        builder.Property(x => x.EmailOrPhoneNumber)
               .IsRequired()
               .HasColumnName("email_or_phone_number");

        builder.HasIndex(t => t.EmailOrPhoneNumber)
           .IsUnique();

        builder.Property(x => x.StringType)
              .IsRequired()
              .HasColumnName("string_type");

        builder.Property(x => x.CreatedDateTime)
            .IsRequired()
             .HasColumnName("created_date_time");
    }
}
