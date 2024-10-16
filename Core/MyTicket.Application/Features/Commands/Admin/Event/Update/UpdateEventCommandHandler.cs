using MediatR;
using Microsoft.Extensions.Options;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Infrastructure.Extensions;
using MyTicket.Infrastructure.Settings;

namespace MyTicket.Application.Features.Commands.Event.Update;
public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserManager _userManager;
    private readonly IOptions<FileSettings> _fileSettings;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IUserManager userManager, IOptions<FileSettings> fileSettings)
    {
        _eventRepository = eventRepository;
        _userManager = userManager;
        _fileSettings = fileSettings;
    }

    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var eventEntity = await _eventRepository.GetAsync(x => x.Id == request.Id);

        if (eventEntity == null)
            throw new Exception("Event not found.");

        // Əsas şəkli yadda saxlamaq
        // Update main event details
        var eventMedias = new List<EventMedia>();
        if (request.MainImage != null)
        {
            (string mainImagePath, string mainImageFileName) = await request.MainImage.SaveAsync(_fileSettings.Value.CreateSubFolders(
                _fileSettings.Value.Path,
                _fileSettings.Value.EventSettings.EntityName,
                request.Name,
                _fileSettings.Value.EventSettings.Media));

            var mainImage = new EventMedia();
            mainImage.SetDetails(mainImageFileName, mainImagePath, MediaType.Image, true);
            eventMedias.Add(mainImage);
        }

        // Add new media files
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

        // Tədbirin məlumatlarını yenilə
        eventEntity.SetDetailsForUpdate(request.Name, request.StartTime, request.EndTime, request.Description, eventMedias, userId);

        // Silinmiş media fayllarını yoxla və sil
        if (request.DeletedMediaIds != null && request.DeletedMediaIds.Any())
        {
            var deletedMedia = eventEntity.EventMedias.Where(m => request.DeletedMediaIds.Contains(m.Id)).ToList();
            if (deletedMedia.Any())
            {
                foreach (var media in deletedMedia)
                {
                   eventEntity.EventMedias.Remove(media);
                }
            }
        }

        // Yenilənmiş tədbiri yadda saxla
        await _eventRepository.Update(eventEntity);
        await _eventRepository.Commit(cancellationToken);

        return true;
    }
}
