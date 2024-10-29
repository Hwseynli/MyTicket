using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.PromoCodes;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Domain.Entities.Orders;
public class Order : BaseEntity
{
    public string OrderCode { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; }
    public DateTime OrderDate { get; private set; }
    public bool IsPaid { get; private set; }
    public decimal TotalAmount { get; private set; }
    public List<Ticket> Tickets { get; private set; }
    public int? PromoCodeId { get; private set; }
    public PromoCode? PromoCode { get; private set; }

    public void SetDetails(string orderCode, int userId, List<Ticket> tickets, int? promoCodeId)
    {
        OrderCode = orderCode;
        UserId = userId;
        OrderDate = DateTime.UtcNow;
        IsPaid = false;
        Tickets = new List<Ticket>();
        PromoCodeId = promoCodeId;
        CalculateTotalAmount();
    }

    public void ApplyPromoCode(PromoCode promoCode)
    {
        if (promoCode == null || !promoCode.IsValid())
            throw new DomainException("Promo kod keçərli deyil.");

        PromoCode = promoCode;
        PromoCodeId = promoCode.Id;

        var discountAmount = TotalAmount * (promoCode.DiscountAmount / 100m);
        TotalAmount -= discountAmount;
    }


    public void AddTicket(Ticket ticket)
    {
        if (ticket == null) throw new DomainException("Bilet mövcud deyil.");
        Tickets.Add(ticket);
        TotalAmount += ticket.Price;
    }

    private void CalculateTotalAmount()
    {
        TotalAmount = Tickets.Sum(t => t.Price);
        if (PromoCode != null && PromoCode.IsValid())
        {
            TotalAmount -= PromoCode.ApplyDiscount(TotalAmount, PromoCode.DiscountType);
        }
    }

    public void MarkAsPaid()
    {
        if (IsPaid)
            throw new DomainException("Order is already paid.");

        IsPaid = true;
    }
}

