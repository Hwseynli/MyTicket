using Microsoft.Extensions.DependencyInjection;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Infrastructure;
public static class ServiceRegistration
{
    public static void AddInfrastructureRegistration(this IServiceCollection services)
    {
        services.AddScoped<BankClient>();
       // services.AddScoped<HttpClient>();
    }
}

