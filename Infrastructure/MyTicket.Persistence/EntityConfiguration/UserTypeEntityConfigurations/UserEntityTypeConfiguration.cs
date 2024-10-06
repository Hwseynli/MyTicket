using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Persistence.EntityConfiguration.UserTypeEntityConfigurations;
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(t => t.Id);

        builder.Property(x => x.Id)
               .HasColumnName("id");

        builder.Property(x => x.FirstName)
               .IsRequired()
               .HasColumnName("firstName");

        builder.Property(x => x.LastName)
               .IsRequired()
               .HasColumnName("lastName");

        builder.Property(x => x.Birthday)
             .HasColumnName("birthday");

        builder.Property(x => x.Gender)   
             .HasColumnName("gender")
             .HasMaxLength(15);

        builder.HasIndex(t => t.PhoneNumber)
            .IsUnique();

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasColumnName("phone_number");

        builder.Property(x => x.Email)
             .IsRequired()
             .HasColumnName("mail");

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.Property(x => x.RoleId)
                .IsRequired()
                .HasColumnName("role_id");

        builder.Property(x => x.PasswordHash)
             .IsRequired()
             .HasColumnName("password_hash");

        builder.Property(x => x.PasswordHash)
             .IsRequired()
             .HasColumnName("password_hash");

        builder.Property(x => x.RefreshToken)
             .HasColumnName("refresh_token");

        builder.Property(x => x.OtpCode)
           .HasColumnName("otp_code");

        builder.Property(x => x.OtpGeneratedTime)
             .HasColumnName("otp_generated_time");

        builder.Property(x => x.Activated)
             .HasDefaultValue(true)
             .HasColumnName("activated");

        builder.Property(x => x.IsDeleted)
             .HasColumnName("isDeleted");

        builder.Property(x => x.LastPasswordChangeDateTime)
             .HasColumnName("last_password_change_date");
    }
}