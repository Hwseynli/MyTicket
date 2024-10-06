using MyTicket.Application.Interfaces.IRepositories;
using MyTicket.Domain.Entities.Users;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}