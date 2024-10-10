using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Users;
using MyTicket.Infrastructure.Utils;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Users;
public class SubscriberRepository : Repository<Subscriber>, ISubscriberRepository
{
    public SubscriberRepository(AppDbContext context) : base(context)
    {
    }

    public bool IsValidEmailOrPhoneNumber(string emailOrPhoneNumber)
    {
        // Simple email and phone number validation
        return Helper.IsEmail(emailOrPhoneNumber) || Helper.IsPhoneNumber(emailOrPhoneNumber);
    }
}

