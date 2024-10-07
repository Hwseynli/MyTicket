using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.User.ForgotPassword.ResetPassword;
using MyTicket.Application.Features.Commands.User.ForgotPassword.SendOtp;
using MyTicket.Application.Features.Commands.User.ForgotPassword.VerifyOtp;
using MyTicket.Application.Features.Commands.User.Login;
using MyTicket.Application.Features.Commands.User.RefreshToken;

namespace MyTicket.AdminPanel.Controllers;
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] CreateAuthorizationTokenCommand command)
    {
        return Ok(await _mediator.Send(command));
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

