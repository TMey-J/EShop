using FluentValidation;

namespace EShop.Application.Features.Order.Requests.Command.Validations;

public class AddToOrderCommandValidation: AbstractValidator<AddToOrderCommandRequest>
{
    public AddToOrderCommandValidation()
    {
        RuleFor(x => x.ProductId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.SellerId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.ColorId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.Quantity).GreaterThan((short)0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.UserId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
