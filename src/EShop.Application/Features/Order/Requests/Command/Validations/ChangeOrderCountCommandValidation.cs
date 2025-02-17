using FluentValidation;

namespace EShop.Application.Features.Order.Requests.Command.Validations;

public class ChangeOrderCountCommandValidation: AbstractValidator<ChangeOrderCountCommandRequest>
{
    public ChangeOrderCountCommandValidation()
    {
        RuleFor(x => x.Quantity).GreaterThan((short)0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.UserId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.OrderDetailId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
