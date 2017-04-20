using Cmas.BusinessLayers.CallOffOrders.Entities;
using FluentValidation;

namespace Cmas.Services.CallOffOrders.Validation
{
    public class RateValidator : AbstractValidator<Rate>
    {
        public RateValidator()
        {
            RuleFor(rate => rate.Id)
                .Must(id => !string.IsNullOrEmpty(id))
                .WithMessage("id cannot be null or empty");
        }
    }
}