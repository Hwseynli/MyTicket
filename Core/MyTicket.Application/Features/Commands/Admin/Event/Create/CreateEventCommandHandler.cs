using MediatR;
using Microsoft.Extensions.Options;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Medias;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.Extensions;
using MyTicket.Infrastructure.Settings;

namespace MyTicket.Application.Features.Commands.Admin.Event.Create;
public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IPlaceHallRepository _placeHallRepository;
    private readonly IUserManager _userManager;
    private readonly IOptions<FileSettings> _fileSettings;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IEmailManager _emailManager;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUserManager userManager, IOptions<FileSettings> fileSettings, IPlaceHallRepository placeHallRepository, ITicketRepository ticketRepository, ISubscriberRepository subscriberRepository, IEmailManager emailManager)
    {
        _eventRepository = eventRepository;
        _userManager = userManager;
        _fileSettings = fileSettings;
        _placeHallRepository = placeHallRepository;
        _ticketRepository = ticketRepository;
        _subscriberRepository = subscriberRepository;
        _emailManager = emailManager;
    }

    public async Task<bool> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();

        var placeHall = await _placeHallRepository.GetAsync(ph => ph.Id == request.PlaceHallId, "Seats");
        if (placeHall == null)
            throw new NotFoundException("Zal tapılmadı.");

        // Zalda eyni zaman aralığında tədbir olub olmadığını yoxlamaq
        var existingEvents = await _eventRepository.GetAllAsync(e => e.PlaceHallId == request.PlaceHallId &&
                                                      (e.StartTime < request.EndTime.AddMinutes(30)) &&
                                                      (request.StartTime.AddMinutes(-30) < e.EndTime));
        if (existingEvents.Any())
            throw new DomainException("Eyni zaman aralığında həmin zalda başqa bir tədbir keçirilir.");

        if (request.EventMediaModels == null || !request.EventMediaModels.Any(m => m.MainImage != null && m.MainImage.IsImage()))
            throw new DomainException("Əsas şəkil əlavə olunmalıdır.");

        // Tədbir məlumatlarını yığmaq
        var newEvent = new Domain.Entities.Events.Event();
        newEvent.SetDetails(request.Title, request.MinPrice, request.StartTime, request.EndTime, request.Description,
                            request.SubCategoryId, request.PlaceHallId, 0,
                            request.Language, request.MinAge, userId);

        for (int i = 0; i < request.EventMediaModels.Count; i++)
        {
            EventMedia eventMedia = new EventMedia();
            eventMedia.SetDetails(userId);

            if (request.EventMediaModels[i].MainImage != null)
            {
                (string path, string fileName) = await request.EventMediaModels[i].MainImage.SaveAsync(_fileSettings.Value.CreateSubFolders(
                     _fileSettings.Value.Path,
                     _fileSettings.Value.EventSettings.EntityName,
                     request.Title,
                     _fileSettings.Value.EventSettings.Medias)
                    );
                var media = new Media();
                media.SetDetails(MediaType.Image, fileName, path, request.EventMediaModels[i].Others, userId, true);
                eventMedia.Medias.Add(media);
            }

            if (request.EventMediaModels[i].Medias != null)
            {
                for (int j = 0; j < request.EventMediaModels[i].Medias?.Count; j++)
                {
                    (string path, string fileName) =
                        await request.EventMediaModels[i].Medias[j].SaveAsync(_fileSettings.Value.CreateSubFolders(
                         _fileSettings.Value.Path,
                         _fileSettings.Value.EventSettings.EntityName,
                         request.Title,
                         _fileSettings.Value.EventSettings.Medias)
                        );

                    var media = new Media();

                    if (request.EventMediaModels[i].Medias[j].IsImage())
                    {
                        media.SetDetails(MediaType.Image, fileName, path, request.EventMediaModels[i].Others, userId);
                    }
                    else if (request.EventMediaModels[i].Medias[j].IsVideo())
                    {
                        media.SetDetails(MediaType.Video, fileName, path, request.EventMediaModels[i].Others, userId);
                    }
                    else
                    {
                        throw new BadRequestException("Media formatı şəkil ya da video olamlıdır");
                    }
                    media.SetAuditDetails(userId);
                    eventMedia.Medias.Add(media);
                }
            }
            newEvent.EventMedias.Add(eventMedia);
        }

        // Tədbiri əlavə edirik
        await _eventRepository.AddAsync(newEvent);
        await _eventRepository.Commit(cancellationToken);

        // Oturacaqlar üzrə bilet yaratma prosesi
        if (placeHall.Seats.Capacity <= 0)
            throw new ValidationException();

        await _ticketRepository.CreateTickets(placeHall.Seats, newEvent.MinPrice, newEvent.Id, userId, cancellationToken);

        IEnumerable<Subscriber> subscribers = await _subscriberRepository.GetAllAsync(x => x.StringType == StringType.Email);
        await _emailManager.SendEmailForSubscribers(subscribers, "New Event", newEvent.Title, newEvent.Description);
        return true;
    }
}

