using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Event.ViewModels;
using MyTicket.Application.Features.Queries.Tag.ViewModels;
using MyTicket.Application.Interfaces.IRepositories.Categories;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Queries.Tag;
public class TagQueries : ITagQueries
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;

    public TagQueries(ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync(null, "Events", "SubCategories");
        return categories.Select(c => new CategoryDto
        {
            Name = c.Name,
            SubCategoryNames = c.SubCategories.Select(x => x.Name).ToList(),
            EventsCount = c.Events.Count
        }).ToList();
    }

    public async Task<IEnumerable<EventViewModel>> GetEventsByCategoryIdAsync(int categoryId)
    {
        var category = await _categoryRepository.GetAsync(
            x => x.Id == categoryId,
            "Events.EventMedias.Medias", "Events.PlaceHall.Place", "Events.Tickets", "Events.SubCategories");

        if (category == null)
            throw new NotFoundException(UIMessage.NotFound("Category Id"));

        return category.Events.Select(e => EventViewModel.MapToViewModel(e));
    }

    public async Task<IEnumerable<EventViewModel>> GetEventsBySubCategoryIdAsync(int subCategoryId)
    {
        var subCategory = await _subCategoryRepository.GetAsync(
            x => x.Id == subCategoryId,
            "Events.EventMedias.Medias", "Events.PlaceHall.Place", "Events.Tickets", "Events.Categories");

        if (subCategory == null)
            throw new NotFoundException(UIMessage.NotFound("SubCategory Id"));

        return subCategory.Events.Select(e => EventViewModel.MapToViewModel(e));
    }

    public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync()
    {
        var categories = await _subCategoryRepository.GetAllAsync(null, "Events", "Categories");
        return categories.Select(sc => new SubCategoryDto
        {
            Name = sc.Name,
            CategoryNames = sc.Categories.Select(cat => cat.Name).ToList(),
            EventsCount = sc.Events.Count
        }).ToList();
    }
}

