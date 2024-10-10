namespace MyTicket.Infrastructure.Utils;
public static class Helper
{
    public static bool IsEmail(string input)
    {
        return input.Contains("@") && input.Contains(".");
    }

    public static bool IsPhoneNumber(string input)
    {
        // Check if it's a valid phone number (basic length check and digit check)
        return input.All(char.IsDigit) && input.Length >= 10 && input.Length <= 15;
    }
}