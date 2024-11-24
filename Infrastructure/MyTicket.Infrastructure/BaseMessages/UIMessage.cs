using Microsoft.Extensions.Localization;

namespace MyTicket.Infrastructure.BaseMessages;
public static class UIMessage
{
    private static IStringLocalizer _localization;
    public static void Configure(IStringLocalizerFactory factory)
    {
        var type = typeof(UIMessage);
        _localization = factory.Create(type);
    }
    public static string NotAccess => _localization["NotAccess"];
    public static string Invalid => _localization["Invalid"];

    public static string GetSuccessMessage => _localization["SuccessOperation"];
    public static string GetFailureMessage => _localization["FailureOperation"];

    public static string PasswordResetMessage(string fieldName, string str)
    {
        return string.Format(_localization["PasswordResetMessage"],fieldName,str);
    }

    public static string NotFound(string fieldName)
    {
        return string.Format(_localization["NotFound"], fieldName);
    }
    public static string Required(string fieldName)
    {
        return string.Format(_localization["Required"], fieldName);
    }
    public static string MaxLength(string fieldName, int maxLength)
    {
        return string.Format(_localization["MaxLength"], fieldName, maxLength);
    }
    public static string MinLength(string fieldName, int minLength)
    {
        return string.Format(_localization["MinLength"], fieldName, minLength);
    }
    public static string GreaterThanZero(string fieldName)
    {
        return string.Format(_localization["GreaterThanZero"], fieldName);
    }
    public static string GreaterThan(string fieldName1, string fieldName2)
    {
        return string.Format(_localization["GreaterThan"], fieldName1, fieldName2);
    }
    public static string NotEmpty(string fieldName)
    {
        return string.Format(_localization["NotEmpty"], fieldName);
    }
    public static string ValidProperty(string propertyName)
    {
        return string.Format(_localization["ValidProperty"], propertyName);
    }
    public static string UniqueProperty(string fieldName)
    {
        return string.Format(_localization["UniqueProperty"], fieldName);
    }
    public static string InvalidImage(string fieldName)
    {
        return string.Format(_localization["InvalidImage"], fieldName);
    }
    public static string AlreadyExist(string fieldName)
    {
        return string.Format(_localization["AlreadyExist"], fieldName);
    }
    public static string Between(string fieldName, int min, int max)
    {
        return string.Format(_localization["Between"], fieldName, min, max);
    }
}

