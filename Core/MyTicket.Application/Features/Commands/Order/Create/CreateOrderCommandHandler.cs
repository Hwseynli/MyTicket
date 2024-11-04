using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Application.Features.Commands.Order.Create;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Domain.Entities.Orders.Order>
{
    private readonly IOrderManager _orderManager;
    private readonly IUserManager _userManager;
    private readonly IEmailManager _emailManager;
    private readonly IOrderRepository _orderRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IPromoCodeRepository _promoCodeRepository;

    public CreateOrderCommandHandler(IUserManager userManager, IOrderRepository orderRepository, ITicketRepository ticketRepository, IBasketRepository basketRepository, IEmailManager emailManager, IOrderManager orderManager, IPromoCodeRepository promoCodeRepository)
    {
        _userManager = userManager;
        _orderRepository = orderRepository;
        _ticketRepository = ticketRepository;
        _basketRepository = basketRepository;
        _emailManager = emailManager;
        _orderManager = orderManager;
        _promoCodeRepository = promoCodeRepository;
    }

    public async Task<Domain.Entities.Orders.Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetCurrentUser();

        var basket = await _basketRepository.GetAsync(x=>x.UserId==user.Id, "TicketsWithTime");
        if (basket == null)
            throw new BadRequestException($"Basket not found for {user.FirstName} {user.LastName}");

        if (!basket.TicketsWithTime.Any())
            throw new NotFoundException("Basket is empty");

        // Ticketlər mövcuddurmu?
        var tickets = new List<Ticket>();
        foreach (var item in basket.TicketsWithTime)
        {
            var ticket = await _ticketRepository.GetAsync(x => x.Id == item.TicketId, "User", "Event");
            if (ticket != null)
            {
                tickets.Add(ticket);
            }
        }

        if (!tickets.Any())
            throw new DomainException("Heç bir bilet tapılmadı.");

        // Sifariş yaratmaq
        var order = new Domain.Entities.Orders.Order();
        order.SetDetails(Guid.NewGuid().ToString(), tickets, user.Id, request.PromoCodeId);

        decimal totalAmount = order.TotalAmount;
        // Promo kod tətbiqi
        if (request.PromoCodeId.HasValue)
        {
            var promoCode = await _orderManager.GetPromoCodeByIdAsync(request.PromoCodeId.Value, user.Id);
            if (promoCode == null || !promoCode.IsValid())
                throw new DomainException("Invalid Promo Code.");
            order.ApplyPromoCode(promoCode);
            promoCode.AddUserForPromoCode(user.Id);
            await _promoCodeRepository.Update(promoCode);
            await _promoCodeRepository.Commit(cancellationToken);
        }

        await _orderManager.Payment(request.Token, request.Email ?? user.Email, user.FirstName, user.LastName, user.PhoneNumber, order.TotalAmount);

        foreach (var item in tickets)
        {
            item.SellTicket(user.Id);
            await _ticketRepository.Update(item);
            await _ticketRepository.Commit(cancellationToken);
        }

        // Ödəniş təsdiqləndi
        order.MarkAsPaid();
        await _orderRepository.AddAsync(order);
        await _orderRepository.Commit(cancellationToken);

        // CreateOrderCommandHandler: Səbətdən biletləri silmək funksionallığı əlavə edilir
        foreach (var item in tickets)
        {
            item.SellTicket(user.Id);
            await _ticketRepository.Update(item);
        }
        // Səbəti boşaldın
        basket.ClearTickets();
      
        await _basketRepository.Update(basket);
        await _basketRepository.Commit(cancellationToken);
        await _ticketRepository.Commit(cancellationToken);
        await _emailManager.SendReceiptAsync(request.Email??user.Email, order,totalAmount - order.TotalAmount);  // Receipt PDF faylını göndərir

        return order;
    }
}