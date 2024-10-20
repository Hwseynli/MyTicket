﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyTicket.Persistence.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyTicket.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyTicket.Domain.Entities.Categories.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_date_time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<DateTime>("RecordDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<int?>("UpdateById")
                        .HasColumnType("integer")
                        .HasColumnName("update_by_id");

                    b.HasKey("Id");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Categories.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_date_time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<DateTime>("RecordDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("record_date_time");

                    b.Property<int?>("UpdateById")
                        .HasColumnType("integer")
                        .HasColumnName("update_by_id");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("sub_categories", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Events.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("AverageRating")
                        .HasColumnType("double precision")
                        .HasColumnName("average_rating");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer")
                        .HasColumnName("created_by_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date_time");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_date_time");

                    b.Property<int>("PlaceHallId")
                        .HasColumnType("integer")
                        .HasColumnName("place_hall_id");

                    b.Property<DateTime>("RecordDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("record_date_time");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date_time");

                    b.Property<int>("SubCategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("sub_category_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int?>("UpdateById")
                        .HasColumnType("integer")
                        .HasColumnName("update_by_id");

                    b.HasKey("Id");

                    b.HasIndex("PlaceHallId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("events", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Events.EventMedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer")
                        .HasColumnName("event_id");

                    b.Property<int>("MediaType")
                        .HasColumnType("integer")
                        .HasColumnName("media_type");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("event_medias", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Medias.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer")
                        .HasColumnName("created_by_id");

                    b.Property<int>("EventMediaId")
                        .HasColumnType("integer")
                        .HasColumnName("event_media_id");

                    b.Property<bool>("IsMain")
                        .HasColumnType("boolean")
                        .HasColumnName("is_main");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_date_time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("path");

                    b.Property<DateTime>("RecordDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("record_date_time");

                    b.Property<int?>("UpdateById")
                        .HasColumnType("integer")
                        .HasColumnName("update_by_id");

                    b.HasKey("Id");

                    b.HasIndex("EventMediaId");

                    b.ToTable("medias", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Places.Place", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("address");

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_date_time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<DateTime>("RecordDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("record_date_time");

                    b.Property<int?>("UpdateById")
                        .HasColumnType("integer")
                        .HasColumnName("update_by_id");

                    b.HasKey("Id");

                    b.ToTable("places", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Places.PlaceHall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_date_time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<int>("PlaceId")
                        .HasColumnType("integer")
                        .HasColumnName("place_id");

                    b.Property<DateTime>("RecordDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("record_date_time");

                    b.Property<int?>("UpdateById")
                        .HasColumnType("integer")
                        .HasColumnName("update_by_id");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.ToTable("place_halls", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Places.Seat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("integer")
                        .HasColumnName("created_by_id");

                    b.Property<DateTime?>("LastUpdateDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_date_time");

                    b.Property<int>("PlaceHallId")
                        .HasColumnType("integer")
                        .HasColumnName("place_hall_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("price");

                    b.Property<DateTime>("RecordDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("record_date_time");

                    b.Property<int>("RowNumber")
                        .HasColumnType("integer")
                        .HasColumnName("row_number");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("integer")
                        .HasColumnName("seat_number");

                    b.Property<string>("SeatType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("seat_type");

                    b.Property<int?>("UpdateById")
                        .HasColumnType("integer")
                        .HasColumnName("update_by_id");

                    b.HasKey("Id");

                    b.HasIndex("PlaceHallId");

                    b.ToTable("seats", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Ratings.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("rated_at");

                    b.Property<int>("RatingValue")
                        .HasColumnType("integer")
                        .HasColumnName("rating_value");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("event_ratings", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Users.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_name");

                    b.HasKey("Id");

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Users.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<string>("EmailOrPhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email_or_phone_number");

                    b.Property<int>("StringType")
                        .HasColumnType("integer")
                        .HasColumnName("string_type");

                    b.HasKey("Id");

                    b.HasIndex("EmailOrPhoneNumber")
                        .IsUnique();

                    b.ToTable("subscribers", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Activated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_activated");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("birthday");

                    b.Property<string>("ConfirmToken")
                        .HasColumnType("text")
                        .HasColumnName("confirm_token");

                    b.Property<DateTime?>("DeletedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_time");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<int?>("Gender")
                        .HasMaxLength(15)
                        .HasColumnType("integer")
                        .HasColumnName("gender");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<DateTime>("LastPasswordChangeDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_password_change_date");

                    b.Property<string>("OtpCode")
                        .HasColumnType("text")
                        .HasColumnName("otp_code");

                    b.Property<DateTime?>("OtpGeneratedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("otp_generated_time");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Categories.SubCategory", b =>
                {
                    b.HasOne("MyTicket.Domain.Entities.Categories.Category", "Category")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Events.Event", b =>
                {
                    b.HasOne("MyTicket.Domain.Entities.Places.PlaceHall", "PlaceHall")
                        .WithMany("Events")
                        .HasForeignKey("PlaceHallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyTicket.Domain.Entities.Categories.SubCategory", "SubCategory")
                        .WithMany("Events")
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlaceHall");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Events.EventMedia", b =>
                {
                    b.HasOne("MyTicket.Domain.Entities.Events.Event", "Event")
                        .WithMany("EventMedias")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Medias.Media", b =>
                {
                    b.HasOne("MyTicket.Domain.Entities.Events.EventMedia", "EventMedia")
                        .WithMany("Medias")
                        .HasForeignKey("EventMediaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventMedia");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Places.PlaceHall", b =>
                {
                    b.HasOne("MyTicket.Domain.Entities.Places.Place", "Place")
                        .WithMany("PlaceHalls")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Places.Seat", b =>
                {
                    b.HasOne("MyTicket.Domain.Entities.Places.PlaceHall", "PlaceHall")
                        .WithMany("Seats")
                        .HasForeignKey("PlaceHallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlaceHall");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Ratings.Rating", b =>
                {
                    b.HasOne("MyTicket.Domain.Entities.Events.Event", "Event")
                        .WithMany("Ratings")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyTicket.Domain.Entities.Users.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Users.User", b =>
                {
                    b.HasOne("MyTicket.Domain.Entities.Users.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Categories.Category", b =>
                {
                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Categories.SubCategory", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Events.Event", b =>
                {
                    b.Navigation("EventMedias");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Events.EventMedia", b =>
                {
                    b.Navigation("Medias");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Places.Place", b =>
                {
                    b.Navigation("PlaceHalls");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Places.PlaceHall", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Seats");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Users.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("MyTicket.Domain.Entities.Users.User", b =>
                {
                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
