using MyTicket.Application.Interfaces.IRepositories.Settings;
using MyTicket.Domain.Entities.Settings;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Settings;
public class SettingRepository : Repository<Setting>, ISettingRepository
{
    public SettingRepository(AppDbContext context) : base(context)
    {
    }
}

