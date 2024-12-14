using EShop.Application.Features.AdminPanel.User.Requests.Queries;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Seller.Requests.Queries.Validations
{
    public class GetSellerQueryValidation : AbstractValidator<GetSellerQueryRequest>
    {
        public GetSellerQueryValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);

        }
    }
}
