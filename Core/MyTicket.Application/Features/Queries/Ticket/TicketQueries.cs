using System;//nizamlay;c; yol ayr;c;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Ticket.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Queries.Basket;
public class TicketQueries : ITicketQueries
{
    private readonly IUserManager _userManager;
    private readonly IBasketRepository _basketRepository;
    private readonly ITicketRepository _ticketRepository;

    public TicketQueries(IUserManager userManager, IBasketRepository basketRepository, ITicketRepository ticketRepository)
    {
        _userManager = userManager;
        _basketRepository = basketRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<List<TicketDto>> GetTicketsInBasket()
    {
        int userId = _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException();

        var basket = await _basketRepository.GetAsync(x => x.UserId == userId, nameof(Domain.Entities.Baskets.Basket.TicketsWithTime));
        if (basket == null)
        {
            basket = new Domain.Entities.Baskets.Basket();
            basket.SetDetails(userId);
            await _basketRepository.AddAsync(basket);
            await _basketRepository.Commit(CancellationToken.None);
        }

        List<TicketDto> dtos = new List<TicketDto>();

        foreach (var item in basket.TicketsWithTime)
        {
            if (item!=null)
            {
                var ticket = await _ticketRepository.GetAsync(x => x.Id == item.TicketId, "Event", "Seat", "Event.PlaceHall");
                dtos.Add(new TicketDto(ticket.Id, ticket.UniqueCode, ticket.Event.Title, ticket.Event.PlaceHall.Name, ticket.Seat.SeatNumber, ticket.Seat.RowNumber, ticket.Price));
            }
        }
        return dtos;
    }
}

