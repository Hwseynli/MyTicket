using MyTicket.Application.Features.Queries.OrderHistory.ViewModels;

namespace MyTicket.Application.Features.Queries.OrderHistory;
public interface IOrderQueries
{
    Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync();
    Task<IEnumerable<OrderViewModel>> GetOrdersByDateAsync(DateTime date);
    Task<IEnumerable<OrderViewModel>> GetOrdersByTicketAsync(int ticketId);
    Task<OrderViewModel> GetLatestOrderAsync();
    Task<IEnumerable<OrderViewModel>> GetOrdersByUpcomingEventAsync();
}

