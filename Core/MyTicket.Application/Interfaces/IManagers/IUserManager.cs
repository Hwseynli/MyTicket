using MyTicket.Domain.Entities.Users;

namespace MyTicket.Application.Interfaces.IManagers;
public interface IUserManager
{
    Task<User> GetCurrentUser(params string[]? includes);
    Task<int> GetCurrentUserId();
    (string token, DateTime expireAt) GenerateTJwtToken(User user);
}

