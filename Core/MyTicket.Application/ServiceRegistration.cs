using FluentValidation;
using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyTicket.Application.Common.Behaviour;
using MyTicket.Application.Features.Queries.User;
using MyTicket.Application.Features.Queries.Admin;

namespace MyTicket.Application;
public static class ServiceRegistration
{
    public static void AddApplicationRegistration(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<IAdminQueries, AdminQueries>();
    }
}