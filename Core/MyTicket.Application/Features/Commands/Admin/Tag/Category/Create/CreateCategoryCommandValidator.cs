using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Categories;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Tag.Category.Create;
public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(UIMessage.Required("Category Name"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Category Name", 3))
            .MaximumLength(100).WithMessage(UIMessage.MaxLength("Category Name", 100))
            .MustAsync(async (name, cancellation) => await _categoryRepository.IsPropertyUniqueAsync(c => c.Name, name)).WithMessage(UIMessage.UniqueProperty("Category Name"));
    }
}

