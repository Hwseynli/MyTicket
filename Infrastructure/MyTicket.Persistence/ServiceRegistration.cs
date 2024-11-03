using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories;
using MyTicket.Persistence.Concrete;
using MyTicket.Persistence.Context;
using MyTicket.Persistence.Repositories;
using MyTicket.Persistence.Services;

namespace MyTicket.Persistence;
public static class ServiceRegistration
{
    public static void AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), options =>
            {
                options.MigrationsHistoryTable("__efmigrationshistory", "testing");
                options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), new List<string>());
            });

        });

        services.Scan(scan => scan
             .FromAssembliesOf(typeof(IRepository<>))
             .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
             .AsImplementedInterfaces()
             .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(Repository<>))
            .AddClasses(classes => classes.AssignableTo(typeof(Repository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IClaimManager, ClaimManager>();
        services.AddScoped<IUserManager, UserManager>();
        //services.AddScoped<IPaymentManager, PaymentManager>();

        services.AddTransient<IEmailManager, EmailManager>();
        services.AddTransient<ISmsManager, SmsManager>();

        services.AddHostedService<HardDeleteBacgroundJob>();
        services.AddHostedService<BasketCleanupBackgroundJob>();
    }
}