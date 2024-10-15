using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Categories;

namespace MyTicket.Persistence.EntityConfiguration.CategoryEntityTypeConfigurations;
public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .HasColumnName("id");

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("name");

        builder.Property(c => c.CreatedById)
               .HasColumnName("created_by_id");

        builder.Property(c => c.UpdateById)
               .HasColumnName("update_by_id");

        builder.Property(c => c.RecordDateTime)
               .HasColumnName("created_date_time");

        builder.Property(c => c.LastUpdateDateTime)
               .HasColumnName("last_update_date_time");

        builder.HasMany(c => c.SubCategories)
               .WithOne(sc => sc.Category)
               .HasForeignKey(sc => sc.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

