using MediatR;

namespace MyTicket.Application.Features.Commands.WishList.Remove;
public class RemoveFromWishListCommand : IRequest<bool>
{
    public int EventId { get; set; }
}