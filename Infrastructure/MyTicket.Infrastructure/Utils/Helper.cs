using System.Text.RegularExpressions;

namespace MyTicket.Infrastructure.Utils;
public static class Helper
{
    public static bool IsEmail(string input)
    {
        return input.Contains("@") && input.Contains(".");
    }

    public static bool IsPhoneNumber(string input)
    {
        // Beynəlxalq telefon nömrəsi üçün regex şərti
        string pattern = @"^\+?[1-9]\d{1,14}$";

        // Telefon nömrəsinin boşluqları və tireləri çıxarılsın
        input = input.Replace(" ", "").Replace("-", "");

        // Regex pattern ilə yoxla
        return Regex.IsMatch(input, pattern);
    }
}