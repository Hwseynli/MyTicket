using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.Basket.RemoveTicket;
public class RemoveTicketFromBasketCommandHandler : IRequestHandler<RemoveTicketFromBasketCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;
    private readonly ITicketRepository _ticketRepository;
    private readonly IBasketRepository _basketRepository;

    public RemoveTicketFromBasketCommandHandler(IUserRepository userRepository, IUserManager userManager, ITicketRepository ticketRepository, IBasketRepository basketRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _ticketRepository = ticketRepository;
        _basketRepository = basketRepository;
    }

    public async Task<bool> Handle(RemoveTicketFromBasketCommand request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var user = await _userRepository.GetAsync(x => x.Id == userId);
        if (user == null)
            throw new UnAuthorizedException();

        var ticket = await _ticketRepository.GetAsync(x => x.Id == request.TicketId);
        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var basket = await _basketRepository.GetAsync(x => x.UserId == userId, "TicketsWithTime");
        if (basket == null)
            throw new NotFoundException("Basket not found.");

        if (!basket.TicketsWithTime.Any(x => x.TicketId == ticket.Id))
            throw new NotFoundException("Ticket not found in Basket");

        basket.RemoveTicket(basket.TicketsWithTime.FirstOrDefault(x => x.TicketId == ticket.Id));
        _basketRepository.Update(basket);
        await _basketRepository.Commit(cancellationToken);

        ticket.ReserveTicket(null, false);
        _ticketRepository.Update(ticket);

        await _ticketRepository.Commit(cancellationToken);

        return true;
    }
}

