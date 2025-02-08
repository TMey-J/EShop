using FluentValidation;

namespace EShop.Application.Features.Product.Requests.Queries.Validations;

public class GetSellersProductQueryValidation: AbstractValidator<GetSellersProductQueryRequest>
{
    public GetSellersProductQueryValidation()
    {
        RuleFor(x => x.Code).NotEmpty()
            .WithMessage(Messages.Validations.Required);
        RuleFor(x => x.ProductId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
