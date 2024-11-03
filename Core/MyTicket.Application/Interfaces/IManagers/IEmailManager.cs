using MyTicket.Domain.Entities.Users;

namespace MyTicket.Application.Interfaces.IManagers;
public interface IEmailManager
{
    Task SendOtpAsync(string email, string otpCode);
    Task SendEmailAsync(string toEmail, string subject, string messageBody);
    Task SendEmailForSubscribers(IEnumerable<Subscriber> subscribers, string subject, string title, string description);
}