namespace MyTicket.Infrastructure.BaseMessages;
public static class UIMessage
{
    public const string SuccessOperation = "{0} successfully {1}.";
    public const string FailureOperation = "Failed to {1} {0}.";

    public static string GetSuccessMessage(string entity, string action)
    {
        return string.Format(SuccessOperation, entity, action);
    }

    public static string GetFailureMessage(string entity, string action)
    {
        return string.Format(FailureOperation, entity, action);
    }
    public static string NotAccess() => "You have not access.";
    public static string NotFound(string fieldName) => $"{fieldName} not found.";
    public static string Required(string fieldName) => $"{fieldName} is required.";
    public static string MaxLength(string fieldName, int maxLength) => $"{fieldName} cannot exceed {maxLength} characters.";
    public static string MinLength(string fieldName, int minLength) => $"{fieldName} cannot be less than {minLength} characters.";
    public static string Invalid() => "Invalid credentials";
    public static string GreaterThanZero(string fieldName) => $"{fieldName} must be greater than 0.";
    public static string GreaterThan(string fieldName1, string fieldName2) => $"{fieldName1} must be greater than {fieldName2}.";
    public static string NotEmpty(string fieldName) => $"{fieldName} cannot be empty.";
    public static string ValidProperty(string propertyName) => $"{propertyName} must be valid.";
    public static string UniqueProperty(string fieldName) => $"{fieldName} must be unique.";
    public static string InvalidImage(string fieldName) => $"{fieldName} must be a valid image.";
    public static string AlreadyExist(string fieldName) => $"{fieldName} is already exsist. ";
    public static string Between(string fieldName, int min, int max) => $"The {fieldName} can be between {min} and {max}.";
}

