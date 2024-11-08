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
using MyTicket.Infrastructure.BaseMessages;
using MyTicket.Application.Interfaces.IRepositories.Categories;

namespace MyTicket.Application.Features.Commands.Admin.Event.Update;
public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, bool>
{
    private readonly IUserManager _userManager;
    private readonly IEventRepository _eventRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITicketManager _ticketManager;
    private readonly IOptions<FileSettings> _fileSettings;
    private readonly IEventMediaRepository _eventMediaRepository;

    public UpdateEventCommandHandler(IUserManager userManager, IEventRepository eventRepository, ITicketManager ticketManager, IOptions<FileSettings> fileSettings, IEventMediaRepository eventMediaRepository, ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository)
    {
        _userManager = userManager;
        _eventRepository = eventRepository;
        _ticketManager = ticketManager;
        _fileSettings = fileSettings;
        _eventMediaRepository = eventMediaRepository;
        _subCategoryRepository = subCategoryRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        // Retrieve event and validate
        var eventEntity = await _eventRepository.GetAsync(e => e.Id == request.Id, "Tickets", "PlaceHall.Seats", "EventMedias.Medias", "SubCategories");
        if (eventEntity == null)
            throw new NotFoundException(UIMessage.NotFound("Event"));

        var category = await _categoryRepository.GetAsync(x=>x.Id==request.CategoryId,"SubCategories");
        if (category == null)
            throw new NotFoundException(UIMessage.NotFound("Category id"));
        // Collect existing SubCategories and check for updates
        var existingSubCategoryIds = eventEntity.SubCategories.Select(sc => sc.Id).ToList();
        var newSubCategoryIds = request.SubCategoryIds.Except(existingSubCategoryIds).ToList();
        var removedSubCategoryIds = existingSubCategoryIds.Except(request.SubCategoryIds).ToList();

        // Get and add new SubCategories
        if (newSubCategoryIds.Any())
        {
            var newSubCategories = await _subCategoryRepository.GetAllAsync(sc => newSubCategoryIds.Contains(sc.Id),"Categories");
            if (newSubCategories.Any())
            {
                foreach (var subCategory in newSubCategories)
                {
                    eventEntity.SubCategories.Add(subCategory);
                    if (!subCategory.Categories.Any(x=>x.Id==category.Id))
                    {
                        category.SubCategories.Add(subCategory);
                    }
                }
                await _categoryRepository.Update(category);
                await _categoryRepository.Commit(cancellationToken);
            }
        }

        // Extract the SubCategories to be removed
        if (removedSubCategoryIds.Any())
        {
            var removedSubCategories = eventEntity.SubCategories.Where(sc => removedSubCategoryIds.Contains(sc.Id)).ToList();
            foreach (var subCategory in removedSubCategories)
            {
                eventEntity.SubCategories.Remove(subCategory);
            }
        }

        // Update event details
        eventEntity.SetDetailsForUpdate(request.Title, request.MinPrice, request.StartTime, request.EndTime, request.Description, eventEntity.EventMedias, request.CategoryId, eventEntity.SubCategories, request.PlaceHallId, eventEntity.AverageRating, request.Language, request.MinAge, userId);

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
                            throw new BadRequestException("Media format must be image or video");
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