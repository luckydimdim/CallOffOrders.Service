using Cmas.BusinessLayers.CallOffOrders.Entities;
using FluentValidation;

namespace Cmas.Services.CallOffOrders.Validation
{
    public class CallOffOrderValidator: AbstractValidator<CallOffOrder>
    {
        public CallOffOrderValidator()
        {
            RuleFor(request => request.Rates).SetCollectionValidator(new RateValidator());
        }
    }
}