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

            RuleFor(request => request.StartDate).Must(d => d.HasValue ? d.Value.Kind == DateTimeKind.Utc : true);

            RuleFor(request => request.FinishDate).Must(d => d.HasValue ? d.Value.Kind == DateTimeKind.Utc : true);

            RuleFor(request => request).Must(r => (r.StartDate.HasValue && r.FinishDate.HasValue)
                ? (r.StartDate < r.FinishDate)
                : true);
        }
    }
}