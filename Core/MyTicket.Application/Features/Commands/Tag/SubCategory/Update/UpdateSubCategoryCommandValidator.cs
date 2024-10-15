using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Categories;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Update;
public class UpdateSubCategoryCommandValidator : AbstractValidator<UpdateSubCategoryCommand>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    public UpdateSubCategoryCommandValidator(ISubCategoryRepository subCategoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;

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