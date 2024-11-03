using FluentValidation;

namespace MyTicket.Application.Features.Commands.User.Delete.SoftDeleteRequest;
public class SoftDeleteRequestCommandValidator : AbstractValidator<SoftDeleteRequestCommand>
{
    public SoftDeleteRequestCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
                .EmailAddress();
    }
}

