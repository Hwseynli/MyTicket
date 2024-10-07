using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.User.ForgotPassword.ResetPassword;
using MyTicket.Application.Features.Commands.User.ForgotPassword.SendOtp;
using MyTicket.Application.Features.Commands.User.ForgotPassword.VerifyOtp;
using MyTicket.Application.Features.Commands.User.Login;
using MyTicket.Application.Features.Commands.User.RefreshToken;
using MyTicket.Application.Features.Commands.User.Register;
using MyTicket.Application.Features.Commands.User.UpdateUser;
using MyTicket.Application.Features.Queries.User;

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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] CreateAuthorizationTokenCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Roles ="User")]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _userQueries.GetUserProfileAsync();
        return Ok(profile);
    }

    [Authorize(Roles = "User")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("User updated successfully.") : BadRequest("User not found or invalid data.");
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    #region ForgotPassword

    [HttpPost("sendOtp")]
    public async Task<IActionResult> SendOtp(SendOtpCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("OTP has been sent to your email.") : BadRequest("User not found");
    }

    [HttpPost("verifyOtp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpCommand command)
    {
        var isValid = await _mediator.Send(command);
        return isValid ? Ok("OTP is valid.") : BadRequest("Invalid or expired OTP.");
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        var isReset = await _mediator.Send(command);
        return isReset ? Ok("Password updated successfully.") : BadRequest("Failed to reset password.");
    }

    #endregion
}