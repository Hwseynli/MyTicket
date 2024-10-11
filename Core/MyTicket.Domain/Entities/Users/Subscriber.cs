using MyTicket.Domain.Common;

namespace MyTicket.Domain.Entities.Users;
public class Subscriber : BaseEntity
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public void SetDetail(string? email=null, string? phoneNumber=null)
    {
        Email = email;
        PhoneNumber = phoneNumber;
        CreatedDateTime = DateTime.UtcNow.AddHours(4);
    }
}