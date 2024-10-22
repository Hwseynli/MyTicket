using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Settings;

namespace MyTicket.Application.Features.Commands.Admin.Setting.Update;
public class UpdateSettingCommandValidator:AbstractValidator<UpdateSettingCommand>
{
    public UpdateSettingCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0).WithMessage("SettingId is required.");
        RuleFor(command => command.Key).NotNull().MinimumLength(3).MaximumLength(150);
        RuleFor(command => command.Value).NotNull().MinimumLength(3).MaximumLength(250);
    }
}

