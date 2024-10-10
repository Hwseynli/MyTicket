using MyTicket.Domain.Entities.Users;

namespace MyTicket.Application.Interfaces.IRepositories.Users;
public interface ISubscriberRepository : IRepository<Subscriber>
{
    bool IsValidEmailOrPhoneNumber(string emailOrPhoneNumber);
}

