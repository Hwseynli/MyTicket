using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Persistence.Services;
public class HardDeleteBacgroundJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HardDeleteBacgroundJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                var usersToDelete = await userRepository.GetAllAsync(u => u.IsDeleted && u.DeletedTime.HasValue
                                                   && u.DeletedTime.Value.AddDays(30) <= DateTime.UtcNow);

                foreach (var user in usersToDelete)
                {
                    await userRepository.HardDelete(user);
                }

                await userRepository.Commit(stoppingToken);
            }

            // Job-u 1 gün sonra yenidən işə salır
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}