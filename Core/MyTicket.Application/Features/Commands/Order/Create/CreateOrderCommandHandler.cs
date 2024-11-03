using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.PromoCodes;
using MyTicket.Domain.Exceptions;
using Stripe;

namespace MyTicket.Application.Features.Commands.Order.Create;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Domain.Entities.Orders.Order>
{
    private readonly IUserManager _userManager;
    private readonly IOrderRepository _orderRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IBasketRepository _basketRepository;

    public CreateOrderCommandHandler(IUserManager userManager, IOrderRepository orderRepository, ITicketRepository ticketRepository, IBasketRepository basketRepository)
    {
        _userManager = userManager;
        _orderRepository = orderRepository;
        _ticketRepository = ticketRepository;
        _basketRepository = basketRepository;
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
            var ticket = await _ticketRepository.GetAsync(x => x.Id == item.TicketId);
            if (ticket != null)
            {
                tickets.Add(ticket);
            }
        }

        if (!tickets.Any())
            throw new DomainException("Heç bir bilet tapılmadı.");

        // Promo kod tətbiqi
        Domain.Entities.PromoCodes.PromoCode? promoCode = null;
        if (request.PromoCodeId.HasValue)
        {
            promoCode = await _orderRepository.GetPromoCodeByIdAsync(request.PromoCodeId.Value);
            if (promoCode == null || !promoCode.IsValid())
                throw new DomainException("Promo kod keçərli deyil.");
        }

        // Sifariş yaratmaq
        var order = new Domain.Entities.Orders.Order();
        order.SetDetails(Guid.NewGuid().ToString(), tickets, user.Id, request.PromoCodeId);

        var optionCust = new CustomerCreateOptions
        {
            Email = request.Email ?? user.Email,
            Name = user.FirstName + " " + user.LastName,
            Phone = user.PhoneNumber
        };
        var serviceCust = new CustomerService();
        Customer customer = serviceCust.Create(optionCust);

        // Stripe ödənişi
        var chargeOptions = new ChargeCreateOptions
        {
            Amount = (long)(order.TotalAmount * 100),
            Currency = "USD",
            Description = "Ticket Order Payment",
            Source = "tok_visa", // Frontend-dən alınan Source burada istifadə olunmalıdır
            ReceiptEmail=request.Email??user.Email
        };

        var serviceCharge = new ChargeService();
        Charge charge = await serviceCharge.CreateAsync(chargeOptions);

        if (charge.Status != "succeeded")
            throw new DomainException("Ödəniş uğursuz oldu.");

        foreach (var item in tickets)
        {
            item.SellTicket(user.Id);
            _ticketRepository.Update(item);
            await _ticketRepository.Commit(cancellationToken);
        }

        // Ödəniş təsdiqləndi
        order.MarkAsPaid();
        await _orderRepository.AddAsync(order);
        await _orderRepository.Commit(cancellationToken);
        //Send Email olmalidir
        return order;
    }
}