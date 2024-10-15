using Microsoft.Extensions.Options;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Infrastructure.Extensions;
using MyTicket.Infrastructure.Settings;

namespace MyTicket.Application.Features.Commands.Event.Create;
public class CreateEventCommandHandler
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserManager _userManager;
    private readonly IOptions<FileSettings> _fileSettings;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
    {
        _eventRepository = eventRepository;
        _userManager = userManager;
        _fileSettings = fileSettings;
    }

    public async Task<bool> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();

        // Əsas şəkli yadda saxlamaq
        (string mainImagePath, string mainImageFileName) = await request.MainImage.SaveAsync(_fileSettings.Value.CreateSubFolders(
            _fileSettings.Value.Path,
            _fileSettings.Value.EventSettings.EntityName,
            request.Name,
            _fileSettings.Value.EventSettings.Media));

        // Event üçün obyekt yaratmaq
        var mainImage = new EventMedia();
        mainImage.SetDetails(mainImageFileName, mainImagePath, MediaType.Image,true);
        var eventMedias = new List<EventMedia>();

        // Digər media faylları
        for (int i = 0; i < request.EventMedias.Count; i++)
        {
            var eventMediaModel = request.EventMedias[i];
            (string mediaPath, string mediaFileName) = await eventMediaModel.File.SaveAsync(_fileSettings.Value.CreateSubFolders(
                _fileSettings.Value.Path,
                _fileSettings.Value.EventSettings.EntityName,
                request.Name,
                _fileSettings.Value.EventSettings.Media));

            var eventMedia = new EventMedia();
            eventMedia.SetDetails(mediaFileName, mediaPath, (MediaType)eventMediaModel.MediaTypeId);
            eventMedia.SetAuditDetails(userId);
            eventMedias.Add(eventMedia);
        }

        // Tədbiri yaratmaq
        var eventEntity = new Domain.Entities.Events.Event();
        eventEntity.SetDetails(request.Name, request.StartTime, request.EndTime, request.Description, eventMedias, request.PlaceHallId, userId, await _eventRepository.GetAllAsync(x=>x.PlaceHallId==request.PlaceHallId));

        // Tədbiri əlavə etmək və yadda saxlamaq
        await _eventRepository.AddAsync(eventEntity);
        await _eventRepository.Commit(cancellationToken);

        return true;
    }
}