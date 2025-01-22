using FluentValidation;

namespace EShop.Application.Features.SellerPanel.Requests.Queries.Validations;

public class GetReservedProductsQueryValidation: AbstractValidator<GetAllReservedProductsQueryRequest>
{
    public GetReservedProductsQueryValidation()
    {
        RuleFor(x => x.Search.Title).MaximumLength(300)
            .WithMessage(Messages.Validations.MaxLength);
        RuleFor(x => x.SellerId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
