using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Categories;

namespace MyTicket.Application.Features.Commands.Tag.Category.Update;
public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
                .WithMessage("Kateqoriya adı müvafiq olmalıdır.");
    }
}

