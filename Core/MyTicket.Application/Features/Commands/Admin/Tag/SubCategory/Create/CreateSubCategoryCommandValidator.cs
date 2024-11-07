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

        RuleFor(sc => sc.CategoryId).NotEmpty().WithMessage(UIMessage.NotEmpty("Category id"))
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Category id"));
    }
}