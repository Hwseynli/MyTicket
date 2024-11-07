using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.User.Delete.SoftDeleteRequest;
public class SoftDeleteRequestCommandValidator : AbstractValidator<SoftDeleteRequestCommand>
{
    public SoftDeleteRequestCommandValidator()
    {
        RuleFor(x => x.Email)
                .NotEmpty().WithMessage(UIMessage.Required("Email"))
                .EmailAddress().WithMessage(UIMessage.ValidProperty("Email"));
    }
}

