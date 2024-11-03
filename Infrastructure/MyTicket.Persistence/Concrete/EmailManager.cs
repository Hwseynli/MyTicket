﻿using Microsoft.Extensions.Configuration;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Domain.Entities.Users;
using System.Net;
using System.Net.Mail;

namespace MyTicket.Persistence.Concrete;
public class EmailManager : IEmailManager
{
    private readonly IConfiguration _configuration;

    public EmailManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendOtpAsync(string toEmail, string otpCode)
    {
        var smtpSettings = _configuration.GetSection("Smtp");

        var smtpClient = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["UserName"]),
            Subject = "Your OTP Code",
            Body = $"Your OTP code is: {otpCode}",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string messageBody)
    {
        var smtpSettings = _configuration.GetSection("Smtp");

        var smtpClient = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"]),
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["UserName"]),
            Subject = subject,
            Body = messageBody,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailForSubscribers(IEnumerable<Subscriber> subscribers, string subject, string title, string description)
    {
        if (subscribers != null && subscribers.Count() > 0)
        {
            foreach (var item in subscribers)
            {
                await SendEmailAsync(
                    item.EmailOrPhoneNumber,
                    $"{subject}",
                    $"<html><body><h1>{title}</h1><p>{description}</p></body></html>"
                );
            }
        }
    }
}