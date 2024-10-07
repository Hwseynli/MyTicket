using Microsoft.AspNetCore.Authorization;
using MyTicket.Application.Features.Queries.Admin.ViewModels;
using MyTicket.Application.Interfaces.IRepositories;

namespace MyTicket.Application.Features.Queries.Admin;
public class AdminQueries : IAdminQueries
{
    private readonly IUserRepository _userRepository;

    public AdminQueries(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync(x => x.RoleId != 1, "Role");
        return UserDto.CreateDtos(users);
    }
}