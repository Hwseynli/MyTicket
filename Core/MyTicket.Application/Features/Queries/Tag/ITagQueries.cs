using MyTicket.Application.Features.Queries.Event.ViewModels;
using MyTicket.Application.Features.Queries.Tag.ViewModels;

namespace MyTicket.Application.Features.Queries.Tag;
public interface ITagQueries
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync();
    Task<IEnumerable<EventViewModel>> GetEventsBySubCategoryIdAsync(int subCategoryId);
    Task<IEnumerable<EventViewModel>> GetEventsByCategoryIdAsync(int categoryId);
}

