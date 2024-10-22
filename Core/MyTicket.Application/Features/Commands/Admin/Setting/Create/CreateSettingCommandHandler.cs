using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Settings;

namespace MyTicket.Application.Features.Commands.Admin.Setting.Create;
public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, bool>
{
    private readonly IUserManager _userManager;
    private readonly ISettingRepository _settingRepository;

    public CreateSettingCommandHandler(IUserManager userManager, ISettingRepository settingRepository)
    {
        _userManager = userManager;
        _settingRepository = settingRepository;
    }

    public async Task<bool> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
    {
        int userId = _userManager.GetCurrentUserId();

        if (userId <= 0 || userId == null)
            throw new UnAuthorizedException();

        var newSetting = new Domain.Entities.Settings.Setting();
        newSetting.SetDetails(request.Key, request.Value, userId);

        await _settingRepository.AddAsync(newSetting);
        await _settingRepository.Commit(cancellationToken);
        return true;
    }
}