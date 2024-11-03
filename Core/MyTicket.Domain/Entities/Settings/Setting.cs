using MyTicket.Domain.Entities.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Settings;
public class Setting:Editable<User>
{
    public string Key { get; private set; }
    public string Value { get; private set; }
    public void SetDetails(string key, string value, int userId)
    {
        Key = key.Capitalize();
        Value = value;
        SetAuditDetails(userId);
    }

    public void SetDetailsForUpdate(string? key, string? value, int userId)
    {
        SetEditFields(userId);
        if (key!=null)
            Key = key.Capitalize();
        if (value!=null)
            Value = value;
    }
}