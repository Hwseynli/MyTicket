using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyTicket.Domain.Entities.Users;
using MyTicket.Persistence.Context;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace MyTicket.Infrastructure.SeedDatas;
public class AppSeedDbContext
{
    public async Task SeedAsync(AppDbContext context, ILogger<AppSeedDbContext> logger)
    {
        var policy = CreatePolicy(logger, nameof(AppSeedDbContext));
        using (context)
        {
            await policy.ExecuteAsync(async () =>
            {
                await RoleSeedAsync(context);
                if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
            });
        }
    }

    private async Task RoleSeedAsync(AppDbContext context)
    {
        var roleJson = File.ReadAllText("./Infrastructure/SeedDatas/role.json");
        var roles = JsonConvert.DeserializeObject<List<Role>>(roleJson);
        foreach (var r in roles)
        {
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == r.Id);
            if (role is not null)
            {
                role.SetDetails(r.Name);
                context.Roles.Update(role);
            }
            else
            {
                context.Roles.Add(r);
            }
        }
    }

    private AsyncRetryPolicy CreatePolicy(ILogger<AppSeedDbContext> logger, string prefix, int retries = 3)
    {
        return Policy.Handle<SqlException>().WaitAndRetryAsync(

            retries,
            retry => TimeSpan.FromSeconds(5),
            (exception, timeSpan, retry, ctx) =>
            {
                logger.LogTrace($"{prefix} Exception {exception.GetType().Name} with message {exception.Message} detected on attemt {retry} of {retries}");
            });
    }
}