using Microsoft.EntityFrameworkCore;
using MyTicket.Domain.Entities.Categories;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Favourites;
using MyTicket.Domain.Entities.Places;
using MyTicket.Domain.Entities.Ratings;
using MyTicket.Domain.Entities.Settings;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Persistence.Context;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<PlaceHall> PlaceHalls { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventMedia> EventMedias { get; set; }
    public DbSet<Rating> EventRatings { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<WishList> WishLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}