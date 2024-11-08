using MediatR;
using Microsoft.Extensions.Options;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Categories;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Medias;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.BaseMessages;
using MyTicket.Infrastructure.Extensions;
using MyTicket.Infrastructure.Settings;

namespace MyTicket.Application.Features.Commands.Admin.Event.Create;
public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly ITicketManager _ticketManager;
    private readonly IPlaceHallRepository _placeHallRepository;
    private readonly IUserManager _userManager;
    private readonly IOptions<FileSettings> _fileSettings;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IEmailManager _emailManager;

    public CreateEventCommandHandler(IEventRepository eventRepository, ITicketManager ticketManager, IPlaceHallRepository placeHallRepository, IUserManager userManager, IOptions<FileSettings> fileSettings, ISubscriberRepository subscriberRepository, IEmailManager emailManager, ISubCategoryRepository subCategoryRepository)
    {
        _eventRepository = eventRepository;
        _ticketManager = ticketManager;
        _placeHallRepository = placeHallRepository;
        _userManager = userManager;
        _fileSettings = fileSettings;
        _subscriberRepository = subscriberRepository;
        _emailManager = emailManager;
        _subCategoryRepository = subCategoryRepository;
    }

    public async Task<bool> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();

        var placeHall = await _placeHallRepository.GetAsync(ph => ph.Id == request.PlaceHallId, "Seats");
        if (placeHall == null)
            throw new NotFoundException(UIMessage.NotFound("Place hall"));

        // Check if there is an event in the hall at the same time slot
        var existingEvents = await _eventRepository.GetAllAsync(e => e.PlaceHallId == request.PlaceHallId &&
        (e.StartTime < request.EndTime.AddMinutes(30)) && (request.StartTime.AddMinutes(-30) < e.EndTime));
        if (existingEvents.Any())
            throw new DomainException("Another event is being held in the same hall at the same time.");

        if (request.EventMediaModels == null || !request.EventMediaModels.Any(m => m.MainImage != null && m.MainImage.IsImage()))
            throw new DomainException(UIMessage.InvalidImage("Main image"));

        // Validate SubCategories and Categories
        var subCategories = await _subCategoryRepository.GetAllAsync(sc => request.SubCategoryIds.Contains(sc.Id));
        if (!subCategories.Any())
            throw new DomainException("Event must be assigned to at least one valid SubCategory.");

        // Ensure all SubCategories belong to at least one Category
        foreach (var subCategory in subCategories)
        {
            if (!subCategory.Categories.Any())
                throw new DomainException($"SubCategory '{subCategory.Name}' must belong to at least one Category.");
        }

        // Collect event data
        var newEvent = new Domain.Entities.Events.Event();
        newEvent.SetDetails(request.Title, request.MinPrice, request.StartTime, request.EndTime, request.Description,
                            request.CategoryId, subCategories, request.PlaceHallId, 0,
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
                        throw new BadRequestException("Media format must be image or video");
                    }
                    media.SetAuditDetails(userId);
                    eventMedia.Medias.Add(media);
                }
            }
            newEvent.EventMedias.Add(eventMedia);
        }

        // Add the event
        await _eventRepository.AddAsync(newEvent);
        await _eventRepository.Commit(cancellationToken);

        // Seat ticket creation process
        if (placeHall.Seats.Capacity <= 0)
            throw new ValidationException();

        await _ticketManager.CreateTickets(placeHall.Seats, newEvent.MinPrice, newEvent.Id, userId, cancellationToken);

        IEnumerable<Subscriber> subscribers = await _subscriberRepository.GetAllAsync(x => x.StringType == StringType.Email);
        await _emailManager.SendEmailForSubscribers(subscribers, "New Event", newEvent.Title, newEvent.Description);
        return true;
    }
}

