using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain;
public class Editable<TUser> : Auditable<TUser> where TUser : User
{
    public int? UpdateById { get; protected set; }
    public DateTime? LastUpdateDateTime { get; protected set; }

    public void SetEditFields(int? updatedById)
    {
        UpdateById = updatedById;
        LastUpdateDateTime = DateTime.UtcNow.AddHours(4);
    }
}