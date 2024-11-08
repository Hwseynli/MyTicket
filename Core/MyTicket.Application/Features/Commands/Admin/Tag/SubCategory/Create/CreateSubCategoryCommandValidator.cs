using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Categories;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Create;
public class CreateSubCategoryCommandValidator : AbstractValidator<CreateSubCategoryCommand>
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    public CreateSubCategoryCommandValidator(ISubCategoryRepository subCategoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;

        RuleFor(sc => sc.Name)
            .NotEmpty().WithMessage(UIMessage.Required("Name"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Name", 3))
            .MaximumLength(100).WithMessage(UIMessage.MaxLength("Name", 100))
            .MustAsync(async (name,cancellationToken)=> await _subCategoryRepository.IsPropertyUniqueAsync(x=>x.Name,name))
            .WithMessage(UIMessage.AlreadyExsist("Name"));

        RuleFor(x => x.CategoryIds)
            .Must(subCategoryIds => subCategoryIds != null && subCategoryIds.Any(id => id > 0))
            .WithMessage("Event must have at least one valid SubCategory.");
    }
}