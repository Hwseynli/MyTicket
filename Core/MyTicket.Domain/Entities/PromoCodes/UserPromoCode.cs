using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.PromoCodes;
public class UserPromoCode : BaseEntity
{
    public int UserId { get; private set; }
    public User User { get; private set; }
    public int PromoCodeId { get; private set; }
    public PromoCode PromoCode { get; private set; }
    public DateTime UsedDate { get; private set; }

    public void SetDetails(int userId, int promoCodeId)
    {
        UserId = userId;
        PromoCodeId = promoCodeId;
        UsedDate = DateTime.UtcNow.AddHours(4);
    }
}

