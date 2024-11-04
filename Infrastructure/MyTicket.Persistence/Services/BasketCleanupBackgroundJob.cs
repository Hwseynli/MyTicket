using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Baskets;

namespace MyTicket.Persistence.Services;
public class BasketCleanupBackgroundJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BasketCleanupBackgroundJob> _logger;

    public BasketCleanupBackgroundJob(IServiceProvider serviceProvider, ILogger<BasketCleanupBackgroundJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanUpExpiredTicketsAsync();
            // Wait for 15 minutes before the next cleanup
            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
        }
    }

    private async Task CleanUpExpiredTicketsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var basketRepository = scope.ServiceProvider.GetRequiredService<IBasketRepository>();
        var ticketRepository = scope.ServiceProvider.GetRequiredService<ITicketRepository>();

        try
        {
            var baskets = await basketRepository.GetAllAsync(includes:nameof(Basket.TicketsWithTime));

            foreach (var basket in baskets)
            {
                var expiredTickets = basket.GetExpiredTickets();
                if (expiredTickets.Any())
                {
                    foreach (var ticketWithTime in expiredTickets)
                    {
                        var isTicket = await ticketRepository.GetAsync(x => x.Id== ticketWithTime.Id);
                        if (isTicket != null)
                        {
                            basket.ClearTickets();
                            isTicket.ReserveTicket(null, false);
                            await ticketRepository.Update(isTicket);
                            await ticketRepository.Commit(CancellationToken.None);
                        }
                    }

                    await basketRepository.Update(basket);
                    await basketRepository.Commit(CancellationToken.None);

                }
            }

            _logger.LogInformation("Basket cleanup completed successfully at {Time}", DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while cleaning up expired tickets");
        }
    }
}
