﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyTicket.Domain.Entities.Places;

namespace MyTicket.Persistence.EntityConfiguration.PlaceEntityTypeConfigurations;
public class PlaceHallEntityTypeConfiguration : IEntityTypeConfiguration<PlaceHall>
{
    public void Configure(EntityTypeBuilder<PlaceHall> builder)
    {
        builder.ToTable("place_halls");
        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.Id)
               .HasColumnName("id");

        builder.Property(ph => ph.Name)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("name");

        builder.Property(ph => ph.PlaceId)
               .HasColumnName("place_id");

        builder.HasMany(ph => ph.Seats)
               .WithOne(s => s.PlaceHall)
               .HasForeignKey(s => s.PlaceHallId)
               .OnDelete(DeleteBehavior.Cascade);

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
