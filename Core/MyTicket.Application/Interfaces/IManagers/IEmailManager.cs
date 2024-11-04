using MyTicket.Domain.Entities.Orders;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Application.Interfaces.IManagers;
public interface IEmailManager
{
    Task SendReceiptAsync(string toEmail, Order order, decimal discountAmount);
    Task SendOtpAsync(string email, string otpCode);
    Task SendEmailAsync(string toEmail, string subject, string messageBody);
    Task SendEmailForSubscribers(IEnumerable<Subscriber> subscribers, string subject, string title, string description);
}