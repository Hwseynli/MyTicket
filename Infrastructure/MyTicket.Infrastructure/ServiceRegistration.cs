using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Infrastructure;
public static class ServiceRegistration
{
    public static void AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<BankClient>();
    }
}

