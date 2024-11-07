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

        RuleFor(sc => sc.CategoryId).NotEmpty().WithMessage(UIMessage.NotEmpty("Category id"))
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Category id"));
    }
}