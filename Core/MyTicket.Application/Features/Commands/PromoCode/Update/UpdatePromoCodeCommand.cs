using MediatR;

namespace MyTicket.Application.Features.Commands.PromoCode.Update;
public class UpdatePromoCodeCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string UniqueCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public byte ExpirationAfterDays { get; set; }
    public int UsageLimit { get; set; }
}

