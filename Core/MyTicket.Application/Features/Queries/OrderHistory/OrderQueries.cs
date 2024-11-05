using Microsoft.AspNetCore.Authorization;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.OrderHistory.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Orders;

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

    // Bütün sifarişləri qaytarır
    public async Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync()
    {
        int userId = await _userManager.GetCurrentUserId();
        var orders = await _orderRepository.GetAllAsync(x => x.UserId == userId, "Tickets", "Tickets.Event", "Tickets.Seat");

        return orders.Select(order => OrderViewModel.SetDetails(order));
    }

    // Tarixə görə sifarişləri qaytarır
    public async Task<IEnumerable<OrderViewModel>> GetOrdersByDateAsync(DateTime date)
    {
        int userId = await _userManager.GetCurrentUserId();
        var orders = await _orderRepository.GetAllAsync(x =>
            x.OrderDate.Date.Date==date.Date
            && x.UserId == userId, "Tickets", "Tickets.Event", "Tickets.Seat");

        return orders.Select(order => OrderViewModel.SetDetails(order));
    }

    // Biletə görə sifarişləri qaytarır
    public async Task<IEnumerable<OrderViewModel>> GetOrdersByTicketAsync(int ticketId)
    {
        int userId = await _userManager.GetCurrentUserId();
        var orders = await _orderRepository.GetAllAsync(x => x.Tickets.Any(t => t.Id == ticketId) && x.UserId == userId, "Tickets", "Tickets.Event", "Tickets.Seat");

        return orders.Select(order => OrderViewModel.SetDetails(order));
    }

    //ən sonuncu orderi qaytarir
    public async Task<OrderViewModel> GetLatestOrderAsync()
    {
        int userId = await _userManager.GetCurrentUserId();

        // İstifadəçinin bütün sifarişlərini alırıq
        var orders = await _orderRepository.GetAllAsync(x => x.UserId == userId, "Tickets", "Tickets.Event", "Tickets.Seat");

        // Sifarişləri azalan sırayla `OrderDate` tarixinə görə əl ilə sıralayırıq
        var orderList = orders.ToList();
        orderList.Sort((order1, order2) => order2.OrderDate.CompareTo(order1.OrderDate));

        // Ən son sifarişi alırıq
        var latestOrder = orderList.FirstOrDefault();

        if (latestOrder == null)
        {
            throw new NotFoundException("No orders found for the user.");
        }

        return OrderViewModel.SetDetails(latestOrder);
    }

    // Yalnız gələcək tarixli tədbirə görə sifarişləri qaytarır
    public async Task<IEnumerable<OrderViewModel>> GetOrdersByUpcomingEventAsync()
    {
        int userId = await _userManager.GetCurrentUserId();
        DateTime currentDate = DateTime.UtcNow;

        // Uzaq tarixdəki (gələcək) tədbirlər üçün sifarişləri gətirir
        var orders = await _orderRepository.GetAllAsync(
            x => x.Tickets.Any(t => t.Event.StartTime > currentDate) && x.UserId == userId,
            "Tickets", "Tickets.Event", "Tickets.Seat"
        );

        // Gələcək tədbirlər üçün sifarişləri sıralayır (ən uzaq tarixi önə çəkir)
        var sortedOrders = orders.ToList();
        sortedOrders.Sort((o1, o2) =>
        {
            var upcomingEventO1 = o1.Tickets.Where(t => t.Event.StartTime > currentDate)
                                             .Max(t => t.Event.StartTime);
            var upcomingEventO2 = o2.Tickets.Where(t => t.Event.StartTime > currentDate)
                                             .Max(t => t.Event.StartTime);
            return upcomingEventO1.CompareTo(upcomingEventO2);
        });

        // Hər bir sifariş üçün detalları qaytarır
        return sortedOrders.Select(order => OrderViewModel.SetDetails(order));
    }

    public async Task<byte[]> GetOrderReceiptAsync(int orderId)
    {
        // İstifadəçi ID-sini əldə edirik
        var userId = await _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException();

        // Sifarişi tapırıq
        var order = await _orderRepository.GetAsync(o => o.Id == orderId && o.UserId == userId,
            "Tickets", "Tickets.User", "Tickets.Event", "Tickets.Seat.PlaceHall.Place", "PromoCode");

        if (order == null)
            throw new NotFoundException("Sifariş tapılmadı.");

        // PDF Qəbzi yaradırıq
        decimal discountAmount = order.PromoCode != null ? order.PromoCode.DiscountAmount : 0;
        var pdfReceipt = _orderManager.GenerateReceipt(order, discountAmount);

        return pdfReceipt;
    }
}

