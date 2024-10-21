using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Medias;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.Extensions;
using MyTicket.Infrastructure.Settings;

namespace MyTicket.Application.Features.Commands.Admin.Event.Create
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, bool>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IPlaceHallRepository _placeHallRepository;
        private readonly IUserManager _userManager;
        private readonly IOptions<FileSettings> _fileSettings;
        private readonly ILogger<CreateEventCommandHandler> _logger;

        public CreateEventCommandHandler(IEventRepository eventRepository, IUserManager userManager, IOptions<FileSettings> fileSettings, ILogger<CreateEventCommandHandler> logger, IPlaceHallRepository placeHallRepository)
        {
            _eventRepository = eventRepository;
            _userManager = userManager;
            _fileSettings = fileSettings;
            _logger = logger;
            _placeHallRepository = placeHallRepository;
        }

        public async Task<bool> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            int userId = _userManager.GetCurrentUserId();
            if (userId <= 0 && userId == null)
            {
                _logger.LogError($"Unauthorized access attempt by user with ID: {userId}.");
                throw new UnAuthorizedException();
            }

            var placeHall = await _placeHallRepository.GetAsync(ph => ph.Id == request.PlaceHallId);

            if (placeHall == null)
                throw new NotFoundException("Zal tapılmadı.");

            // Zalda eyni zaman aralığında tədbir olub olmadığını yoxlamaq
            var existingEvents = await _eventRepository.GetAllAsync(e => e.PlaceHallId == request.PlaceHallId &&
                                                          (e.StartTime < request.EndTime.AddMinutes(30)) &&
                                                          (request.StartTime.AddMinutes(-30) < e.EndTime));
            if (existingEvents.Any())
            {
                _logger.LogError($"Zalda ({request.PlaceHallId}) eyni zaman aralığında başqa bir tədbir var.");
                throw new DomainException("Eyni zaman aralığında həmin zalda başqa bir tədbir keçirilir.");
            }

            if (request.EventMediaModels == null || request.EventMediaModels.All(m => m.MainImage == null))
                throw new DomainException("Əsas şəkil əlavə olunmalıdır.");

            // Tədbir məlumatlarını yığmaq
            var newEvent = new Domain.Entities.Events.Event();
            newEvent.SetDetails(request.Title, request.StartTime, request.EndTime, request.Description,
                                request.SubCategoryId, request.PlaceHallId, request.InitialRatingValue ?? 0,
                                request.Language, request.MinAge);
            newEvent.SetAuditDetails(userId);

            for (int i = 0; i < request.EventMediaModels.Count; i++)
            {
                EventMedia eventMedia = new EventMedia();
                eventMedia.SetDetails((MediaType)request.EventMediaModels[i].MediaTypeId);
                eventMedia.SetAuditDetails(userId);
                if (request.EventMediaModels[i].MainImage != null)
                {
                    (string path, string fileName) = await request.EventMediaModels[i].MainImage.SaveAsync(_fileSettings.Value.CreateSubFolders(
                         _fileSettings.Value.Path,
                         _fileSettings.Value.EventSettings.EntityName,
                         request.Title,
                         _fileSettings.Value.EventSettings.Medias)
                        );
                    var media = new Media();
                    media.SetDetails(fileName, path, true);
                    media.SetAuditDetails(userId);
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
                        media.SetDetails(fileName, path);
                        media.SetAuditDetails(userId);
                        eventMedia.Medias.Add(media);
                    }
                }
                newEvent.EventMedias.Add(eventMedia);
            }
            // Tədbiri əlavə edirik
            await _eventRepository.AddAsync(newEvent);
            await _eventRepository.Commit(cancellationToken);

            _logger.LogInformation($"Event {newEvent.Title} created successfully.");
            return true;
        }
    }
}

