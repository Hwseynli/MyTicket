﻿using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.PromoCode.Create
{
    public class CreatePromoCodeCommandValidator : AbstractValidator<CreatePromoCodeCommand>
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        public CreatePromoCodeCommandValidator(IPromoCodeRepository promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            RuleFor(x => x.UniqueCode)
                    .Matches(@"^[a-zA-Z0-9]{5,10}$")
                .WithMessage("Unique code must be alphanumeric and 5-10 characters long.")
                    .MustAsync((async (uniqueCode, cancellationToken) => await _promoCodeRepository.IsPropertyUniqueAsync(x => x.UniqueCode, uniqueCode)))
                .WithMessage("UniqueCode already exsist");

            RuleFor(x => x.DiscountAmount)
                    .GreaterThan(0)
                .WithMessage("Discount amount must be greater than 0.");

            RuleFor(x => x.DiscountAmount)
                    .LessThanOrEqualTo(100)
                    .When(x => x.DiscountType == DiscountType.Percent)
                .WithMessage("Percentage discount cannot exceed 100%.");

            RuleFor(x => x.ExpirationAfterDays)
                .NotEmpty()
                .WithMessage("Expiration date must be correct.");

            RuleFor(x => x.UsageLimit)
                    .GreaterThan(0)
                .WithMessage("Usage limit must be greater than 0.");
        }
    }
}

