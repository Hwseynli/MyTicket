using Microsoft.AspNetCore.Authorization;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.OrderHistory.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Queries.OrderHistory;
[Authorize]
public class OrderQueries : IOrderQueries
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderManager _orderManager;
    private readonly IUserManager _userManager;
    public OrderQueries(IOrderRepository orderRepository, IUserManager userManager, IOrderManager orderManager)
    {
        _orderRepository = orderRepository;
        _userManager = userManager;
        _orderManager = orderManager;
    }

    public async Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync()
    {
        int userId = await _userManager.GetCurrentUserId();
        var orders = await _orderRepository.GetAllAsync(x => x.UserId == userId, "Tickets", "Tickets.Event", "Tickets.Seat");

        return orders.Select(order => OrderViewModel.SetDetails(order));
    }

    public async Task<IEnumerable<OrderViewModel>> GetOrdersByDateAsync(DateTime date)
    {
        int userId = await _userManager.GetCurrentUserId();
        var orders = await _orderRepository.GetAllAsync(x =>
            x.OrderDate.Date.Date==date.Date
            && x.UserId == userId, "Tickets", "Tickets.Event", "Tickets.Seat");

        return orders.Select(order => OrderViewModel.SetDetails(order));
    }

    public async Task<IEnumerable<OrderViewModel>> GetOrdersByTicketAsync(int ticketId)
    {
        int userId = await _userManager.GetCurrentUserId();
        var orders = await _orderRepository.GetAllAsync(x => x.Tickets.Any(t => t.Id == ticketId) && x.UserId == userId, "Tickets", "Tickets.Event", "Tickets.Seat");

        return orders.Select(order => OrderViewModel.SetDetails(order));
    }

    public async Task<OrderViewModel> GetLatestOrderAsync()
    {
        int userId = await _userManager.GetCurrentUserId();

        var orders = await _orderRepository.GetAllAsync(x => x.UserId == userId, "Tickets", "Tickets.Event", "Tickets.Seat");

        var orderList = orders.ToList();
        orderList.Sort((order1, order2) => order2.OrderDate.CompareTo(order1.OrderDate));

        var latestOrder = orderList.FirstOrDefault();

        if (latestOrder == null)
            throw new NotFoundException(UIMessage.NotFound("Orders"));

        return OrderViewModel.SetDetails(latestOrder);
    }

    public async Task<IEnumerable<OrderViewModel>> GetOrdersByUpcomingEventAsync()
    {
        int userId = await _userManager.GetCurrentUserId();
        DateTime currentDate = DateTime.UtcNow;

        var orders = await _orderRepository.GetAllAsync(
            x => x.Tickets.Any(t => t.Event.StartTime > currentDate) && x.UserId == userId,
            "Tickets", "Tickets.Event", "Tickets.Seat"
        );

        var sortedOrders = orders.ToList();
        sortedOrders.Sort((o1, o2) =>
        {
            var upcomingEventO1 = o1.Tickets.Where(t => t.Event.StartTime > currentDate)
                                             .Max(t => t.Event.StartTime);
            var upcomingEventO2 = o2.Tickets.Where(t => t.Event.StartTime > currentDate)
                                             .Max(t => t.Event.StartTime);
            return upcomingEventO1.CompareTo(upcomingEventO2);
        });

        return sortedOrders.Select(order => OrderViewModel.SetDetails(order));
    }

    public async Task<byte[]> GetOrderReceiptAsync(int orderId)
    {
        var userId = await _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException(UIMessage.NotAccess());

        var order = await _orderRepository.GetAsync(o => o.Id == orderId && o.UserId == userId,
            "Tickets", "Tickets.User", "Tickets.Event", "Tickets.Seat.PlaceHall.Place", "PromoCode");

        if (order == null)
            throw new NotFoundException(UIMessage.NotFound("Order"));

        // Creating a PDF Receipt
        decimal discountAmount = order.PromoCode != null ? order.PromoCode.DiscountAmount : 0;
        var pdfReceipt = _orderManager.GenerateReceipt(order, discountAmount);

        return pdfReceipt;
    }
}

