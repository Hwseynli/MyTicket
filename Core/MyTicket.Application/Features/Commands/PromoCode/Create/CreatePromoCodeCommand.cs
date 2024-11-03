using MediatR;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.PromoCode.Create;
public class CreatePromoCodeCommand : IRequest<bool>
{
    public string UniqueCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public DiscountType DiscountType { get; set; }
    public byte ExpirationAfterDays { get; set; }
    public int UsageLimit { get; set; }
    public bool IsActive { get; set; }
}

