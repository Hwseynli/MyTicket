using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Categories;

namespace MyTicket.Application.Features.Commands.Tag.Category.Create;
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Kateqoriya adı müvafiq olmalıdır.")
            .MustAsync(async (name, cancellation) => await _categoryRepository.IsPropertyUniqueAsync(c => c.Name, name)).WithMessage("Artıq bu adda category mövcuddur");
    }
}

