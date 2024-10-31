using MediatR;
using Microsoft.Extensions.Configuration;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.PromoCodes;
using MyTicket.Domain.Exceptions;
using Stripe;

namespace MyTicket.Application.Features.Commands.Order.Create;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Domain.Entities.Orders.Order>
{
    private readonly IUserManager _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IConfiguration _configuration;

    public CreateOrderCommandHandler(IUserManager userManager, IUserRepository userRepository, IOrderRepository orderRepository, ITicketRepository ticketRepository, IBasketRepository basketRepository, IConfiguration configuration)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _ticketRepository = ticketRepository;
        _basketRepository = basketRepository;
        _configuration = configuration;
    }

    public async Task<Domain.Entities.Orders.Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        int userId = _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException("userId sıfırdan böyük olmalıdır");

        var user = await _userRepository.GetAsync(x=>x.Id==userId);
        if (user == null)
            throw new UnAuthorizedException("User not found");

        var basket = await _basketRepository.GetAsync(x=>x.UserId==userId, "TicketsWithTime");
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
        PromoCode promoCode = null;
        if (request.PromoCodeId.HasValue)
        {
            promoCode = await _orderRepository.GetPromoCodeByIdAsync(request.PromoCodeId.Value);
            if (promoCode == null || !promoCode.IsValid())
                throw new DomainException("Promo kod keçərli deyil.");
        }

        // Sifariş yaratmaq
        var order = new Domain.Entities.Orders.Order();
        order.SetDetails(Guid.NewGuid().ToString(), tickets, userId, request.PromoCodeId);

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
            item.SellTicket(userId);
            _ticketRepository.Update(item);
            await _ticketRepository.Commit(cancellationToken);
        }

        // Ödəniş təsdiqləndi
        order.MarkAsPaid();
        await _orderRepository.AddAsync(order);
        await _orderRepository.Commit(cancellationToken);
        return order;
    }
}