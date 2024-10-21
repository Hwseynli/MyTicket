﻿using FluentValidation;

namespace MyTicket.Application.Features.Commands.Admin.Rating.GetQuery;
public class GetEventRatingQueryValidator : AbstractValidator<GetEventRatingQuery>
{
    public GetEventRatingQueryValidator()
    {
        RuleFor(command => command.EventId).GreaterThan(0).WithMessage("UserId is required.");
    }
}

