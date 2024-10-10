namespace MyTicket.Application.Interfaces.IManagers;
public interface IEmailManager
{
    Task SendOtpAsync(string email, string otpCode);
    Task SendEmailAsync(string toEmail, string subject, string messageBody);
}