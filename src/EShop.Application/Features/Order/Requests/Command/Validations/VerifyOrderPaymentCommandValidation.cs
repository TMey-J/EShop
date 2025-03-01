using FluentValidation;

namespace EShop.Application.Features.Order.Requests.Command.Validations;

public class VerifyOrderPaymentCommandValidation: AbstractValidator<VerifyOrderPaymentCommandRequest>
{
    public VerifyOrderPaymentCommandValidation()
    {
        RuleFor(x => x.OrderId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.Authority).NotEmpty()
            .WithMessage(Messages.Validations.Required);
    }
}
