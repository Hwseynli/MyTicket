using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Medias;
using MyTicket.Infrastructure.Settings;
using MyTicket.Infrastructure.Extensions;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.Admin.Event.Update;
public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, bool>
{
    private readonly IUserManager _userManager;
    private readonly IEventRepository _eventRepository;
    private readonly ITicketManager _ticketManager;
    private readonly IOptions<FileSettings> _fileSettings;
    private readonly IEventMediaRepository _eventMediaRepository;

    public UpdateEventCommandHandler(IUserManager userManager, IEventRepository eventRepository, ITicketManager ticketManager, IOptions<FileSettings> fileSettings, IEventMediaRepository eventMediaRepository)
    {
        _userManager = userManager;
        _eventRepository = eventRepository;
        _ticketManager = ticketManager;
        _fileSettings = fileSettings;
        _eventMediaRepository = eventMediaRepository;
    }

    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        // Retrieve event and validate
        var eventEntity = await _eventRepository.GetAsync(e => e.Id == request.Id, "Tickets", "PlaceHall.Seats", "EventMedias.Medias");
        if (eventEntity == null)
            throw new NotFoundException("Event not found.");

        // Update event details
        eventEntity.SetDetailsForUpdate(request.Title, request.MinPrice, request.StartTime, request.EndTime, request.Description, eventEntity.EventMedias, request.SubCategoryId, request.PlaceHallId, eventEntity.AverageRating, request.Language, request.MinAge, userId);

        if (request.DeletedMediaIds != null)
        {
            var Medias = await _eventMediaRepository.GetAllAsync(x => request.DeletedMediaIds.Contains(x.Id));
            await _eventMediaRepository.RemoveRange(Medias);
        }

        for (int i = 0; i < request.EventMediaModels?.Count; i++)
        {
            if (request.EventMediaModels[i].Id == 0)
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
                        IFormFile medias = request.EventMediaModels[i].Medias[j];
                        (string path, string fileName) =
                            await medias.SaveAsync(_fileSettings.Value.CreateSubFolders(
                             _fileSettings.Value.Path,
                             _fileSettings.Value.EventSettings.EntityName,
                             request.Title,
                             _fileSettings.Value.EventSettings.Medias)
                            );

                        var media = new Media();

                        if (medias.IsImage())
                        {
                            media.SetDetails(MediaType.Image, fileName, path, request.EventMediaModels[i].Others, userId);
                        }
                        else if (medias.IsVideo())
                        {
                            media.SetDetails(MediaType.Video, fileName, path, request.EventMediaModels[i].Others, userId);
                        }
                        else
                        {
                            throw new BadRequestException("Media formatı şəkil ya da video olamlıdır");
                        }
                        eventMedia.Medias.Add(media);
                    }
                }
                eventEntity.EventMedias.Add(eventMedia);
            }
        }

        await _eventRepository.Update(eventEntity);
        await _eventRepository.Commit(cancellationToken);

        // Check if event is deleted and handle tickets
        if (eventEntity.IsDeleted)
        {
            await _ticketManager.CreateTickets(eventEntity.PlaceHall.Seats, eventEntity.MinPrice, eventEntity.Id, userId, cancellationToken);
        }
        return true;
    }
}