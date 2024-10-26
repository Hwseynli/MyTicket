using MediatR;

namespace MyTicket.Application.Features.Commands.WishList.Add;
public class AddWishListCommand : IRequest<bool>
{
    public int EventId { get; set; }
}

