using MyTicket.Application.Features.Queries.User.ViewModels;

namespace MyTicket.Application.Features.Queries.User;
public interface IUserQueries
{
    Task<UserProfileDto> GetUserProfileAsync();
}

