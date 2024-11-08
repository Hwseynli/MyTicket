using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Categories;

namespace MyTicket.Persistence.EntityConfiguration.CategoryEntityTypeConfigurations;
public class SubCategoryEntityTypeConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable("sub_categories");

        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Id)
               .HasColumnName("id");

        builder.Property(sc => sc.Name)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("name");

        builder.HasIndex(sc => sc.Name)
                .IsUnique();

        builder.Property(sc => sc.CreatedById)
               .HasColumnName("created_by_id");

        builder.Property(sc => sc.UpdateById)
               .HasColumnName("update_by_id");

        builder.Property(sc => sc.RecordDateTime)
               .HasColumnName("record_date_time");

        builder.Property(sc => sc.LastUpdateDateTime)
               .HasColumnName("last_update_date_time");
    }
}

