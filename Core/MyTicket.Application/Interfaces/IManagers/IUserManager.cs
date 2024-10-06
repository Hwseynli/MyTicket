using MyTicket.Domain.Entities.Users;

namespace MyTicket.Application.Interfaces.IManagers;
public interface IUserManager
{
    public int GetCurrentUserId();
    (string token, DateTime expireAt) GenerateTJwtToken(User user);
}

