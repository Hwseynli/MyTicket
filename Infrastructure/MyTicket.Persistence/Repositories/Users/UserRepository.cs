using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Users;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Users;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}