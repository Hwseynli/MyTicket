using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Domain;
public class Auditable<TUser> : BaseEntity where TUser : User
{
    public int CreatedById { get; protected set; }
    public DateTime RecordDateTime { get; protected set; }

    public void SetAuditDetails(int createdById)
    {
        if (CreatedById != 0 && CreatedById != createdById)
        {
            throw new DomainException("CreatedBy already set.");
        }
        CreatedById = createdById;
        RecordDateTime = DateTime.UtcNow.AddHours(4);
    }
}