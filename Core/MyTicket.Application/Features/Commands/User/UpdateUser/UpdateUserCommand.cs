﻿using MediatR;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.User.UpdateUser;
public class UpdateUserCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }  // To check the current password
}

