﻿using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Basket.RemoveTicket;
public class RemoveTicketFromBasketCommandHandler : IRequestHandler<RemoveTicketFromBasketCommand, bool>
{
    private readonly IUserManager _userManager;
    private readonly ITicketRepository _ticketRepository;
    private readonly IBasketRepository _basketRepository;

    public RemoveTicketFromBasketCommandHandler(IUserManager userManager, ITicketRepository ticketRepository, IBasketRepository basketRepository)
    {
        _userManager = userManager;
        _ticketRepository = ticketRepository;
        _basketRepository = basketRepository;
    }

    public async Task<bool> Handle(RemoveTicketFromBasketCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetCurrentUser();

        var ticket = await _ticketRepository.GetAsync(x => x.Id == request.TicketId);
        if (ticket == null)
            throw new NotFoundException(UIMessage.NotFound("ticket"));

        var basket = await _basketRepository.GetAsync(x => x.UserId == user.Id, "TicketsWithTime");
        if (basket == null)
            throw new NotFoundException(UIMessage.NotFound("Basket"));

        if (!basket.TicketsWithTime.Any(x => x.TicketId == ticket.Id))
            throw new NotFoundException(UIMessage.NotFound("Ticket")+"in Basket");

        basket.RemoveTicket(ticket.Id);
        await _basketRepository.Update(basket);
        await _basketRepository.Commit(cancellationToken);

        ticket.ReserveTicket(null, false);
        await _ticketRepository.Update(ticket);

        await _ticketRepository.Commit(cancellationToken);

        return true;
    }
}

