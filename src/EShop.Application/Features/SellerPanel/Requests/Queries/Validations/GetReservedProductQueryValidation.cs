using FluentValidation;

namespace EShop.Application.Features.SellerPanel.Requests.Queries.Validations;

public class GetReservedProductsQueryValidation: AbstractValidator<GetReservedProductQueryRequest>
{
    public GetReservedProductsQueryValidation()
    {
        RuleFor(x => x.ProductId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        
        RuleFor(x => x.ColorId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        
        RuleFor(x => x.SellerId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
