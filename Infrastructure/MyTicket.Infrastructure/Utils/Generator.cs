using System.Security.Cryptography;

namespace MyTicket.Infrastructure.Utils;
public static class Generator
{
    public static string GenerateOtp()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    public static string GenerateConfirmToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    public static string GenerateUniqueCode()
    {
        return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper();
    }

    public static object GenerateRandomNumber()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}