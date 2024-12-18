﻿using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.PromoCodes;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.BaseMessages;

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

    public void SetDetails(string orderCode, List<Ticket> tickets, int userId, int? promoCodeId)
    {
        OrderCode = orderCode;
        UserId = userId;
        OrderDate = DateTime.UtcNow;
        IsPaid = false;
        Tickets = tickets;
        PromoCodeId = promoCodeId;
        CalculateTotalAmount();
    }

    public void ApplyPromoCode(PromoCode promoCode)
    {
        if (promoCode == null || !promoCode.IsValid())
            throw new DomainException(UIMessage.ValidProperty("Promo code"));
        PromoCode = promoCode;
        PromoCodeId = promoCode.Id;
        if (promoCode.DiscountType==Enums.DiscountType.Percent)
        {
            var discountAmount = TotalAmount * (promoCode.DiscountAmount / 100m);
            TotalAmount -= discountAmount;
        }
        else
        {
            TotalAmount -= promoCode.DiscountAmount;
        }
    }

    public void AddTicket(Ticket ticket)
    {
        if (ticket == null) throw new DomainException(UIMessage.NotFound("Ticket"));
        Tickets.Add(ticket);
        TotalAmount += ticket.Price;
    }

    private void CalculateTotalAmount()
    {
        TotalAmount = Tickets.Sum(t => t.Price);
        if (PromoCode != null && PromoCode.IsValid())
        {
            TotalAmount -= PromoCode.CalculateDiscount(TotalAmount, PromoCode.DiscountType);
        }
    }

    public void MarkAsPaid()
    {
        if (IsPaid)
            throw new DomainException("Order already is paid");

        IsPaid = true;
    }
}

