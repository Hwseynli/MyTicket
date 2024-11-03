using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Commands.Basket.AddTicket;
public class AddTicketToBasketCommandHandler : IRequestHandler<AddTicketToBasketCommand, bool>
{
    private readonly IUserManager _userManager;
    private readonly ITicketRepository _ticketRepository;
    private readonly IBasketRepository _basketRepository;

    public AddTicketToBasketCommandHandler(ITicketRepository ticketRepository, IBasketRepository basketRepository, IUserManager userManager)
    {
        _ticketRepository = ticketRepository;
        _basketRepository = basketRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(AddTicketToBasketCommand request, CancellationToken cancellationToken)
    {
        var user = _userManager.GetCurrentUser();
        
        var ticket = await _ticketRepository.GetAsync(x => x.Id == request.TicketId&&x.IsSold==false);
        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        if (ticket.IsReserved&&ticket.UserId!=user.Id)
            throw new BadRequestException("Ticket is reserved");

       var basket = await _basketRepository.GetAsync(x => x.UserId == user.Id, nameof(Domain.Entities.Baskets.Basket.TicketsWithTime));
        if (basket == null)
        {
            basket = new Domain.Entities.Baskets.Basket();
            basket.SetDetails(user.Id);
            await _basketRepository.AddAsync(basket);
            await _basketRepository.Commit(cancellationToken);
        }

        if (basket.TicketsWithTime.Any(x => x.TicketId == ticket.Id))
            throw new BadRequestException("Ticket is existed.");

        basket.AddTicket(ticket.Id);
        _basketRepository.Update(basket);
        await _basketRepository.Commit(cancellationToken);

        ticket.ReserveTicket(user.Id,true);
        _ticketRepository.Update(ticket);
        await _ticketRepository.Commit(cancellationToken);
        return true;
    }
}
