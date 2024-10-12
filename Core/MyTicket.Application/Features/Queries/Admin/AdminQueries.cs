using Microsoft.AspNetCore.Authorization;
using MyTicket.Application.Features.Queries.Admin.ViewModels;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Queries.Admin;
[Authorize]
public class AdminQueries : IAdminQueries
{
    private readonly IUserRepository _userRepository;
    private readonly ISubscriberRepository _subscriberRepository;

    public AdminQueries(IUserRepository userRepository, ISubscriberRepository subscriberRepository)
    {
        _userRepository = userRepository;
        _subscriberRepository = subscriberRepository;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync(x => x.RoleId != 1, "Role");
        return UserDto.CreateDtos(users);
    }

    public async Task<List<SubscriberDto>> GetSubscribersAsync()
    {
        var subscribers = await _subscriberRepository.GetAllAsync();
        return SubscriberDto.CreateDtos(subscribers);
    }
}