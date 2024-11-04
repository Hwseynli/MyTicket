using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Settings;

namespace MyTicket.Application.Features.Commands.Admin.Setting.Update;
public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, bool>
{
    private readonly IUserManager _userManager;
    private readonly ISettingRepository _settingRepository;

    public UpdateSettingCommandHandler(IUserManager userManager, ISettingRepository settingRepository)
    {
        _userManager = userManager;
        _settingRepository = settingRepository;
    }

    public async Task<bool> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        var setting = await _settingRepository.GetAsync(x=>x.Id==request.Id);
        if (setting == null)
            throw new NotFoundException();
        if (!(await _settingRepository.IsPropertyUniqueAsync(x => x.Key, request.Key, request.Id)))
            throw new ValidationException();
        setting.SetDetailsForUpdate(request.Key,request.Value,userId);
        await _settingRepository.Update(setting);
        await _settingRepository.Commit(cancellationToken);
        return true;
    }
}

