using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Persistence.Services;
public class TicketEventCleanupBackgroundJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TicketEventCleanupBackgroundJob> _logger;

    public TicketEventCleanupBackgroundJob(IServiceProvider serviceProvider, ILogger<TicketEventCleanupBackgroundJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanUpExpiredTicketsAndEventsAsync();
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Wait hour before the next cleanup
        }
    }

    private async Task CleanUpExpiredTicketsAndEventsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var ticketRepository = scope.ServiceProvider.GetRequiredService<ITicketRepository>();
        var eventRepository = scope.ServiceProvider.GetRequiredService<IEventRepository>();

        try
        {
            // Get events that have ended but are not yet deleted
            var expiredEvents = await eventRepository.GetAllAsync(e => e.EndTime <= DateTime.UtcNow && !e.IsDeleted, "Tickets");

            foreach (var expiredEvent in expiredEvents)
            {
                // Soft delete the event
                expiredEvent.SoftDelete(expiredEvent.CreatedById);

                // Get and soft delete related tickets
                var expiredTickets = expiredEvent.Tickets.Where(t => !t.IsSold);
                await eventRepository.Update(expiredEvent);
                await eventRepository.Commit(CancellationToken.None);
                await ticketRepository.RemoveRange(expiredTickets);
                await ticketRepository.Commit(CancellationToken.None);
            }

            _logger.LogInformation("Expired events and tickets cleanup completed at {Time}", DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the expired events and tickets cleanup");
        }
    }
}

