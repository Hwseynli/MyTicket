using FluentValidation;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Create;
public class CreateSubCategoryCommandValidator : AbstractValidator<CreateSubCategoryCommand>
{
    public CreateSubCategoryCommandValidator()
    {
        RuleFor(sc => sc.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Sub-kateqoriya adı müvafiq olmalıdır.");

        RuleFor(sc => sc.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId valid olmalıdır.");
    }
}