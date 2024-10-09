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

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _adminQueries.GetUsersAsync();
        if (users.Capacity <= 0)
            throw new NotFoundException();
        return Ok(users);
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignUsersRole(AssignRolesCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(new { message = "Role assigned successfully" }) : BadRequest(new { message = "Failed to assign role" });
    }
}