using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.User.Delete.SoftDeleteRequest;
using MyTicket.Application.Features.Commands.User.ForgotPassword.ResetPassword;
using MyTicket.Application.Features.Commands.User.ForgotPassword.SendOtp;
using MyTicket.Application.Features.Commands.User.ForgotPassword.VerifyOtp;
using MyTicket.Application.Features.Commands.User.Login;
using MyTicket.Application.Features.Commands.User.RefreshToken;
using MyTicket.Application.Features.Commands.User.Register;
using MyTicket.Application.Features.Commands.User.Subscriber.Create;
using MyTicket.Application.Features.Commands.User.UpdateUser;
using MyTicket.Application.Features.Queries.User;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Controllers;
[ApiController]
[Route("api/user")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserQueries _userQueries;

    public UsersController(IMediator mediator, IUserQueries userQueries)
    {
        _mediator = mediator;
        _userQueries = userQueries;
    }

    // POST: api/subscriber
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] CreateSubscriberCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("Subscription", "completed"))
            : BadRequest(UIMessage.GetFailureMessage("Subscription", "complete"));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result=await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("User", "registered"))
            : BadRequest(UIMessage.GetFailureMessage("User", "register"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] CreateAuthorizationTokenCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize]
    [HttpGet("get-profile")]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _userQueries.GetUserProfileAsync();
        return Ok(profile);
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("User", "updated"))
            : BadRequest(UIMessage.GetFailureMessage("User", "update"));
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("soft-delete")]
    public async Task<IActionResult> SoftDelete([FromBody] SoftDeleteRequestCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("User", "soft deleted"))
            : BadRequest(UIMessage.GetFailureMessage("User", "soft delete"));
    }

    #region ForgotPassword

    [HttpPost("sendOtp")]
    public async Task<IActionResult> SendOtp(SendOtpCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("OTP", "sent"))
            : NotFound(UIMessage.NotFound("User"));
    }

    [HttpPost("verifyOtp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpCommand command)
    {
        var isValid = await _mediator.Send(command);
        return isValid ? Ok(UIMessage.GetSuccessMessage("OTP", "verified"))
            : BadRequest(UIMessage.GetFailureMessage("OTP", "verify"));
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        var isReset = await _mediator.Send(command);
        return isReset ? Ok(UIMessage.GetSuccessMessage("Password", "reset"))
            : BadRequest(UIMessage.GetFailureMessage("Password", "reset"));
    }

    #endregion
}

