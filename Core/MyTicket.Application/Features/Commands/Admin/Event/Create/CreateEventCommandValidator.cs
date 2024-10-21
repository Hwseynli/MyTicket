using FluentValidation;
using MyTicket.Application.Features.Commands.Admin.Event.ViewModels;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Infrastructure.Extensions;

namespace MyTicket.Application.Features.Commands.Admin.Event.Create;
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    private readonly IEventRepository _eventRepository;
    public CreateEventCommandValidator(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;

        RuleFor(x => x.Title)
              .NotEmpty().WithMessage("Tədbirin adı boş ola bilməz.")
              .MustAsync(async (title, cancellationToken) => await _eventRepository.IsPropertyUniqueAsync(x=>x.Title,title))
                .WithMessage("Bu başlıqda tədbir yaradılıb istəyirsinizsə update edin ya da başqa ad ilə yaradın")
              .MaximumLength(100).WithMessage("Tədbirin adı maksimum 100 simvol ola bilər.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Tədbirin başlanma vaxtı boş ola bilməz.")
            .GreaterThan(DateTime.Now).WithMessage("Tədbirin başlanma vaxtı keçmiş tarix ola bilməz.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("Tədbirin bitmə vaxtı boş ola bilməz.")
            .GreaterThan(x => x.StartTime).WithMessage("Tədbirin bitmə vaxtı başlanma vaxtından sonra olmalıdır.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Tədbirin təsviri boş ola bilməz.")
            .MaximumLength(500).WithMessage("Tədbirin təsviri maksimum 500 simvol ola bilər.");

        RuleFor(x => x.MinAge)
            .NotEmpty().WithMessage("Tədbirin min neçə yaşında şəxslər üçün olduğu yazılmalıdır. ");

        RuleFor(x => x.PlaceHallId)
            .GreaterThan(0).WithMessage("Zal ID-si düzgün olmalıdır.");

        RuleFor(x => x.SubCategoryId)
            .GreaterThan(0).WithMessage("Alt kateqoriya ID-si düzgün olmalıdır.");

        RuleFor(x => x.InitialRatingValue)
            .GreaterThanOrEqualTo(0).When(x => x.InitialRatingValue.HasValue)
            .WithMessage("İlkin reytinq dəyəri mənfi ola bilməz.");

        RuleForEach(x => x.EventMediaModels)
            .SetValidator(new EventMediaModelValidator());
    }
}

public class EventMediaModelValidator : AbstractValidator<EventMediaModel>
{
    public EventMediaModelValidator()
    {
        RuleFor(x => x.MediaTypeId)
            .GreaterThan(-1).WithMessage("Media növü düzgün olmalıdır.");

        RuleFor(x => x.MainImage)
            .NotNull().WithMessage("Əsas şəkil boş ola bilməz.")
            .Must((mainImage) => mainImage.IsImage()).WithMessage("Main image mütləq şəkil olmalıdır")
            .When(x => x.Medias == null || !x.Medias.Any())
                .WithMessage("Əgər başqa media faylları əlavə edilməyibsə, əsas şəkil olmalıdır.");
    }
}