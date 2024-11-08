using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Queries.Tag;

namespace MyTicket.Controllers;
[Route("api/categories")]
[ApiController]
public class CategoriesController : Controller
{
    private readonly ITagQueries _tagQueries;

    public CategoriesController(ITagQueries tagQueries)
    {
        _tagQueries = tagQueries;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _tagQueries.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("subcategories")]
    public async Task<IActionResult> GetSubCategories()
    {
        var subCategories = await _tagQueries.GetSubCategoriesAsync();
        return Ok(subCategories);
    }

    [HttpGet("subcategories/{subCategoryId}/events")]
    public async Task<IActionResult> GetEventsBySubCategoryId(int subCategoryId)
    {
        var events = await _tagQueries.GetEventsBySubCategoryIdAsync(subCategoryId);
        return Ok(events);
    }

    [HttpGet("categories/{categoryId}/events")]
    public async Task<IActionResult> GetEventsByCategoryId(int categoryId)
    {
        var events = await _tagQueries.GetEventsByCategoryIdAsync(categoryId);
        return Ok(events);
    }
}

