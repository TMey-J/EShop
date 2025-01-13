using EShop.Application.Features.AdminPanel.User.Requests.Commands;
using FluentValidation;

namespace EShop.Application.Features.SellerPanel.Requests.Commands.Validations;

public class ReserveProductCommandValidation: AbstractValidator<ReserveProductCommandRequest>
{
    public ReserveProductCommandValidation()
    {
        RuleFor(x => x.ProductId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        
        RuleFor(x => x.ColorCode).NotEmpty()
            .WithMessage(Messages.Validations.Required);
        
        RuleFor(x => x.BasePrice).GreaterThan((uint)0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        
        RuleFor(x => x.Count).GreaterThan((short)0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
