using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Settings;

namespace MyTicket.Application.Features.Commands.Admin.Setting.Create;
public class CreateSettingCommandValidator:AbstractValidator<CreateSettingCommand>
{
    private readonly ISettingRepository _settingRepository;
    public CreateSettingCommandValidator(ISettingRepository settingRepository)
    {
        _settingRepository = settingRepository;

        RuleFor(command => command.Key).NotNull().MinimumLength(3).MaximumLength(150).MustAsync(async (key,cancellationToken)=>await _settingRepository.IsPropertyUniqueAsync(x=>x.Key.ToLower(),key.Trim().ToLower()));
        RuleFor(command => command.Value).NotNull().MinimumLength(3).MaximumLength(250);
    }
}