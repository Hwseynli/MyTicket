using MyTicket.Application.Features.Queries.User.ViewModels;
using MyTicket.Application.Interfaces.IManagers;

namespace MyTicket.Application.Features.Queries.User;
public class UserQueries : IUserQueries
{
    private readonly IUserManager _userManager;

    public UserQueries(IUserManager userManager)
    {
        _userManager = userManager;
    }
    public async Task<UserProfileDto> GetUserProfileAsync()
    {
        var user = await _userManager.GetCurrentUser();

        return new UserProfileDto
        {
            FistName = user.FirstName,
            LastName = user.LastName,
            Gender=user.Gender.ToString(),
            Birthday=user.Birthday,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        };
    }
}

