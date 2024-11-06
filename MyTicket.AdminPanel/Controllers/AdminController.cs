using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Commands.Admin.User.AssignRoles;
using MyTicket.Application.Features.Queries.Admin;

namespace MyTicket.AdminPanel.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAdminQueries _adminQueries;

    public AdminController(IAdminQueries adminQueries, IMediator mediator)
    {
        _adminQueries = adminQueries;
        _mediator = mediator;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _adminQueries.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("subscribers")]
    public async Task<IActionResult> GetAllSubscriberssAsync()
    {
        var subscribers = await _adminQueries.GetSubscribersAsync();
        return Ok(subscribers);
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignUsersRole(AssignRolesCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(new { message = "Role assigned successfully"}) : BadRequest(new { message = "Failed to assign role" });
    }
}