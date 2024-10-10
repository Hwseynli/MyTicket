using System.Security.Cryptography;

namespace MyTicket.Infrastructure.Utils;
public static class Generator
{
    public static string GenerateOtp()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString(); // 6 rəqəmli OTP yaradılır
    }

    public static string GenerateConfirmToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}