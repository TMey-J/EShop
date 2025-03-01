using FluentValidation;

namespace EShop.Application.Features.Order.Requests.Command.Validations;

public class PayOrderCommandValidation: AbstractValidator<PayOrderCommandRequest>
{
    public PayOrderCommandValidation()
    {
        RuleFor(x => x.UserId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.OrderId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.CallbackUrl).NotEmpty()
            .WithMessage(Messages.Validations.Required);
    }
}
