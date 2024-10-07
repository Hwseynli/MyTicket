using MyTicket.Application.Features.Queries.Admin.ViewModels;

namespace MyTicket.Application.Features.Queries.Admin;
public interface IAdminQueries
{
    Task<List<UserDto>> GetUsersAsync();
}