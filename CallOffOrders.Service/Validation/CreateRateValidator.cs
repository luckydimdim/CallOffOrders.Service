﻿using System.Linq;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using FluentValidation;

namespace Cmas.Services.CallOffOrders.Validation
{
    public class CreateRateValidator : AbstractValidator<CreateRateRequest>
    {
        public CreateRateValidator()
        {
            RuleFor(rate => rate.Currency)
                .Must(s => !string.IsNullOrEmpty(s))
                .WithMessage("Currency cannot be null or empty");

            RuleFor(rate => rate.UnitName)
                .Must(s => new []{ "день", "час" }.Contains(s))
                .WithMessage("Incorrect UnitName");
        }
    }
}