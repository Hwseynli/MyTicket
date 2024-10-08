using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Admin;

namespace MyTicket.AdminPanel.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminQueries _adminQueries;

    public AdminController(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _adminQueries.GetUsersAsync();
        if (users.Capacity <= 0)
            throw new NotFoundException();
        return Ok(users);
    }
}

