using System;
using System.Linq;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using FluentValidation;

namespace Cmas.Services.CallOffOrders.Validation
{
    public class UpdateCallOffOrderValidator : AbstractValidator<UpdateCallOffOrderRequest>
    {
        public UpdateCallOffOrderValidator()
        {
            RuleFor(request => request.CurrencySysName).Must(cur => new[] {"USD", "EUR", "JPY", "RUR"}.Contains(cur));

            RuleFor(request => request.StartDate).Must(d => d.Kind == DateTimeKind.Utc);

            RuleFor(request => request.FinishDate).Must(d => d.Kind == DateTimeKind.Utc);
        }
    }
}