using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Users;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Users;
public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }
}