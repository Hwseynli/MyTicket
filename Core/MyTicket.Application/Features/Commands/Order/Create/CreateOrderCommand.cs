using MediatR;

namespace MyTicket.Application.Features.Commands.Order.Create;
public class CreateOrderCommand : IRequest<string>
{
    public string? Email { get; set; } = null;

    public string? Token { get; set; } = "tok_visa"; //for stripe

    public int? PromoCodeId { get; set; } = null;
}

