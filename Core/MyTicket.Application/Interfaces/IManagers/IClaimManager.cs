using System.Security.Claims;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Application.Interfaces.IManagers;
public interface IClaimManager
{
    int GetCurrentUserId();
    IEnumerable<Claim> GetUserClaims(User user);
}

