using Cmas.Services.CallOffOrders.Dtos.Requests;
using FluentValidation;

namespace Cmas.Services.CallOffOrders.Validation
{
    public class CreateCallOffOrderValidator : AbstractValidator<CreateCallOffOrderRequest>
    {
        public CreateCallOffOrderValidator()
        {
            RuleFor(request => request.ContractId).Must(s => !string.IsNullOrEmpty(s))
                .WithMessage("ContractId cannot be empty");
        }
    }
}