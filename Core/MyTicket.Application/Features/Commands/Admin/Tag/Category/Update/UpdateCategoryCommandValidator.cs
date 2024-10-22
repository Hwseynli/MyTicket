using FluentValidation;

namespace MyTicket.Application.Features.Commands.Tag.Category.Update;
public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0).WithMessage("CategoryId is required.");
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
                .WithMessage("Kateqoriya adı müvafiq olmalıdır.");
    }
}

