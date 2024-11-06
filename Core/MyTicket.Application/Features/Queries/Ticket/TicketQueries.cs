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

    public async Task<List<TicketDto>> GetSoldTicketsForEvent(int eventId)
    {
        var tickets = await _ticketRepository.GetAllAsync(x => x.EventId == eventId && x.IsSold, "Event", "Seat", "Event.PlaceHall");
        var dtos = tickets.Select(x => new TicketDto(x.Id, x.UniqueCode, x.Event.Title, x.Event.PlaceHall.Name, x.Seat.SeatNumber, x.Seat.RowNumber, x.Price, x.IsSold, x.IsReserved)).ToList();
        return dtos;
    }

    public async Task<List<TicketDto>> GetAllTicketsForEvent(int eventId)
    {
        var tickets = await _ticketRepository.GetAllAsync(x => x.EventId == eventId, "Event", "Seat", "Event.PlaceHall");
        var dtos = tickets.Select(x => new TicketDto(x.Id, x.UniqueCode, x.Event.Title, x.Event.PlaceHall.Name, x.Seat.SeatNumber, x.Seat.RowNumber, x.Price, x.IsSold, x.IsReserved)).ToList();
        return dtos;
    }

    public async Task<List<TicketDto>> GetReservedTicketsForEvent(int eventId)
    {
        var tickets = await _ticketRepository.GetAllAsync(x => x.EventId == eventId && x.IsReserved, "Event", "Seat", "Event.PlaceHall");
        var dtos = tickets.Select(x => new TicketDto(x.Id, x.UniqueCode, x.Event.Title, x.Event.PlaceHall.Name, x.Seat.SeatNumber, x.Seat.RowNumber, x.Price, x.IsSold, x.IsReserved)).ToList();
        return dtos;
    }

    public async Task<List<TicketDto>> GetTicketsInBasket()
    {
        int userId = await _userManager.GetCurrentUserId();
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
                dtos.Add(new TicketDto(ticket.Id, ticket.UniqueCode, ticket.Event.Title, ticket.Event.PlaceHall.Name, ticket.Seat.SeatNumber, ticket.Seat.RowNumber, ticket.Price, ticket.IsSold, ticket.IsReserved));
            }
        }
        return dtos;
    }
}

