using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Admin.Setting.Create;
using MyTicket.Application.Features.Commands.Admin.Setting.Update;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.AdminPanel.Controllers;
[Route("api/settings")]
[ApiController]
[Authorize(Roles = "Admin")]
public class SettingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-setting")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateSettingCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("setting", "created"))
        : BadRequest(UIMessage.GetFailureMessage("setting", "create"));
    }

    // PUT api/values/5
    [HttpPut("update-setting")]
    public async Task<IActionResult> UpdateAsync([FromForm] UpdateSettingCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("setting", "updated"))
        : BadRequest(UIMessage.GetFailureMessage("setting", "update"));
    }
}