
namespace MyTicket.Application.Interfaces.IManagers;
public interface ISmsManager
{
    Task SendSmsAsync(string phone, string subject, string body);
}

