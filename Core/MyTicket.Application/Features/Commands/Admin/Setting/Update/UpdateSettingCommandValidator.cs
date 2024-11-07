using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Admin.Setting.Update;
public class UpdateSettingCommandValidator:AbstractValidator<UpdateSettingCommand>
{
    public UpdateSettingCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Id"));
        RuleFor(command => command.Key).NotNull().WithMessage(UIMessage.Required("Key"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Key",3))
            .MaximumLength(150).WithMessage(UIMessage.MaxLength("Key",150));
        RuleFor(command => command.Value).NotNull().WithMessage(UIMessage.Required("Value"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Value", 3))
            .MaximumLength(250).WithMessage(UIMessage.MaxLength("Value", 250));
    }
}

