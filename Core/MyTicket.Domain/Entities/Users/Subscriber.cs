using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Domain.Entities.Users;
public class Subscriber : BaseEntity
{
    public string EmailOrPhoneNumber { get; private set; }
    public StringType StringType { get; private set; }

    public DateTime CreatedDateTime { get; set; }

    public void SetDetail(string emailOrPhoneNumber, StringType stringType)
    {
        EmailOrPhoneNumber = emailOrPhoneNumber;
        StringType = stringType;
        CreatedDateTime = DateTime.UtcNow.AddHours(4);
    }
}