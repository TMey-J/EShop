using FluentValidation;

namespace EShop.Application.Features.SellerPanel.Requests.Commands.Validations;

public class UpdateReservedProductCommandValidation: AbstractValidator<UpdateReservedProductCommandRequest>
{
    public UpdateReservedProductCommandValidation()
    {
        RuleFor(x => x.ProductId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.SellerId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.ColorId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        
        RuleFor(x => x.BasePrice).GreaterThan((uint)0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        
        RuleFor(x => x.Count).GreaterThan((short)0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
