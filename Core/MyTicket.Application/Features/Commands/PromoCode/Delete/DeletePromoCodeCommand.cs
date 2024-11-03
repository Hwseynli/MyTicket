using MediatR;

namespace MyTicket.Application.Features.Commands.PromoCode.Delete;
public class DeletePromoCodeCommand : IRequest<bool>
{
    public int Id { get; set; }
}

