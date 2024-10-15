using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Tag.Category.Create;
using MyTicket.Application.Features.Commands.Tag.Category.Update;
using MyTicket.Application.Features.Commands.Tag.SubCategory.Create;
using MyTicket.Application.Features.Commands.Tag.SubCategory.Update;

namespace MyTicket.AdminPanel.Controllers;
[Route("api/categories")]
[ApiController]
[Authorize(Roles ="Admin")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-category")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
            var result = await _mediator.Send(command);
            return result ? Ok("Category created successfully.") : BadRequest("Category creation failed.");
    }

    [HttpPut("update-category")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command)
    {
            var result = await _mediator.Send(command);
            return result ? Ok("Category updated successfully.") : NotFound("Category not found.");
    }

    [HttpPost("create-sub_category")]
    public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategoryCommand command)
    {
            var result = await _mediator.Send(command);
            return result ? Ok("SubCategory created successfully.") : BadRequest("SubCategory creation failed.");
    }

    [HttpPut("update-sub_category")]
    public async Task<IActionResult> UpdateSubCategory([FromBody] UpdateSubCategoryCommand command)
    {
            var result = await _mediator.Send(command);
            return result ? Ok("SubCategory updated successfully.") : NotFound("SubCategory not found.");
    }
}