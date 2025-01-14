using FluentValidation;

namespace EShop.Application.Features.SellerPanel.Requests.Queries.Validations;

public class SearchProductQueryValidation: AbstractValidator<SearchProductQueryRequest>
{
    public SearchProductQueryValidation()
    {
        RuleFor(x => x.Title).NotEmpty()
            .WithMessage(Messages.Validations.GreaterThanZero)
            .MaximumLength(300)
            .WithMessage(Messages.Validations.MaxLength);
    }
}
