using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Admin;

namespace MyTicket.AdminPanel.Controllers;
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAdminQueries _adminQueries;

    public UsersController(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _adminQueries.GetUsersAsync();
        if (users.Capacity <= 0)
            throw new NotFoundException();
        return Ok(users);
    }
}