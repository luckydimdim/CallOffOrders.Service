﻿using System.Linq;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using FluentValidation;

namespace Cmas.Services.CallOffOrders.Validation
{
    public class UpdateCallOffOrderValidator : AbstractValidator<UpdateCallOffOrderRequest>
    {
        public UpdateCallOffOrderValidator()
        {
            RuleFor(request => request.CurrencySysName).Must(cur => new [] {"USD", "EUR","JPY", "RUR" }.Contains(cur) );
        }
    }
}