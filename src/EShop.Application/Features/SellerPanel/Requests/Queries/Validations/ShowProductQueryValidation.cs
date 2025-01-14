using FluentValidation;

namespace EShop.Application.Features.SellerPanel.Requests.Queries.Validations;

public class ShowProductQueryValidation: AbstractValidator<ShowProductQueryRequest>
{
    public ShowProductQueryValidation()
    {
        RuleFor(x => x.Id).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
