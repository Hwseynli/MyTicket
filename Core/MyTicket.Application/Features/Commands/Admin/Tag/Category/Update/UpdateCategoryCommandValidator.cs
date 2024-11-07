using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Tag.Category.Update;
public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Id"));
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(UIMessage.Required("Name"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Name", 3))
            .MaximumLength(100).WithMessage(UIMessage.MaxLength("Name",100));
    }
}

