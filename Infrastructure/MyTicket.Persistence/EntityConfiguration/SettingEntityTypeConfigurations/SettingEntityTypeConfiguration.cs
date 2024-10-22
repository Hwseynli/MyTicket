using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Settings;

namespace MyTicket.Persistence.EntityConfiguration.SettingEntityTypeConfigurations;
public class SettingEntityTypeConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("settings");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .HasColumnName("id");

        builder.Property(p => p.Key)
               .IsRequired()
               .HasMaxLength(150)
               .HasColumnName("key");

        builder.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("value");

        builder.Property(x => x.CreatedById)
            .HasColumnName("created_by_id");

        builder.Property(x => x.RecordDateTime)
            .HasColumnName("record_date_time");

        builder.Property(x => x.UpdateById)
            .HasColumnName("update_by_id");

        builder.Property(x => x.LastUpdateDateTime)
            .HasColumnName("last_update_date_time");
    }
}

