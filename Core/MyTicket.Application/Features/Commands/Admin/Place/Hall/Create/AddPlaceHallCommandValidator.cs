﻿using FluentValidation;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create;
public class AddPlaceHallCommandValidator : AbstractValidator<AddPlaceHallCommand>
{
    public AddPlaceHallCommandValidator()
    {
        RuleFor(ph => ph.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(ph => ph.SeatCount)
            .GreaterThan(0)
            .WithMessage("SeatCount valid olmalıdır.")
            .Must((command, seatCount) => seatCount % command.RowCount == 0)
            .WithMessage("SeatCount RowCount-a tam bölünməlidir.");

        RuleFor(ph => ph.RowCount)
            .GreaterThan(0)
            .WithMessage("RowCount valid olmalıdır.");

        RuleFor(ph => ph.PlaceId)
            .GreaterThan(0)
            .WithMessage("PlaceId valid olmalıdır.");
    }
}

