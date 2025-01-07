using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Product.Requests.Queries.Validations
{
    public class GetProductQueryValidation : AbstractValidator<GetProductQueryRequest>
    {
        public GetProductQueryValidation()
        {
            RuleFor(x => x.Id).
                GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);

        }
    }
}
