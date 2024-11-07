using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Persistence.Services;
public class HardDeleteBacgroundJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<HardDeleteBacgroundJob> _logger;

    public HardDeleteBacgroundJob(IServiceScopeFactory serviceScopeFactory, ILogger<HardDeleteBacgroundJob> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    var usersToDelete = await userRepository.GetAllAsync(u => u.IsDeleted && u.DeletedTime.HasValue
                                                       && u.DeletedTime.Value.AddDays(30) <= DateTime.UtcNow);

                    foreach (var user in usersToDelete)
                    {
                        await userRepository.HardDelete(user);
                        _logger.LogInformation("User with ID {UserId} hard deleted.", user.Id);
                    }

                    await userRepository.Commit(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while trying to hard delete users.");
                }
            }

            // Restarts the Job after 1 day
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}