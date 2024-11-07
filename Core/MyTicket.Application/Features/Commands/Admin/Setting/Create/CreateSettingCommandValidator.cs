using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Settings;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Admin.Setting.Create;
public class CreateSettingCommandValidator:AbstractValidator<CreateSettingCommand>
{
    private readonly ISettingRepository _settingRepository;
    public CreateSettingCommandValidator(ISettingRepository settingRepository)
    {
        _settingRepository = settingRepository;

        RuleFor(command => command.Key)
            .NotNull().WithMessage(UIMessage.Required("Key"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Key", 3))
            .MaximumLength(150).WithMessage(UIMessage.MaxLength("Key", 150))
            .MustAsync(async (key, cancellationToken) => await _settingRepository.IsPropertyUniqueAsync(x => x.Key.ToLower(), key.Trim().ToLower())).WithMessage(UIMessage.UniqueProperty("Key"));

        RuleFor(command => command.Value)
            .NotNull().WithMessage(UIMessage.Required("Value"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Value", 3))
            .MaximumLength(250).WithMessage(UIMessage.MaxLength("Value", 250));
    }
}