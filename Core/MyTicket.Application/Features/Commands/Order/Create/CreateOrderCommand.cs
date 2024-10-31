using MediatR;

namespace MyTicket.Application.Features.Commands.Order.Create;
public class CreateOrderCommand : IRequest<Domain.Entities.Orders.Order>
{
    public string? Email { get; set; } = null;

    public string? Token { get; set; } = null;

    public int? PromoCodeId { get; set; } = null;
}

