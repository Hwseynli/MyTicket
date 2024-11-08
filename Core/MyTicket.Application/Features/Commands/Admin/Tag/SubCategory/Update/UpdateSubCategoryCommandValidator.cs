using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Tag.SubCategory.Update;
public class UpdateSubCategoryCommandValidator : AbstractValidator<UpdateSubCategoryCommand>
{
    public UpdateSubCategoryCommandValidator()
    {
        RuleFor(sc => sc.Id).NotEmpty().WithMessage(UIMessage.Required("Id"))
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Id"));

        RuleFor(sc => sc.Name)
            .NotEmpty().WithMessage(UIMessage.Required("Name"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Name", 3))
            .MaximumLength(100).WithMessage(UIMessage.MaxLength("Name", 100));

        RuleFor(x => x.CategoryIds)
             .Must(subCategoryIds => subCategoryIds != null && subCategoryIds.Any(id => id > 0))
             .WithMessage("Event must have at least one valid SubCategory.");
    }
}