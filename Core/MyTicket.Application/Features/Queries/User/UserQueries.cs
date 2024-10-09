using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.User.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories;

namespace MyTicket.Application.Features.Queries.User;
public class UserQueries : IUserQueries
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public UserQueries(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }
    public async Task<UserProfileDto> GetUserProfileAsync()
    {
        //var id = _userManager.GetCurrentUserId();
        var user = await _userRepository.GetAsync((u => u.Id == _userManager.GetCurrentUserId()));
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        return new UserProfileDto
        {
            FistName = user.FirstName,
            LastName = user.LastName,
            Gender=user.Gender,
            Birthday=user.Birthday,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        };
    }
}

