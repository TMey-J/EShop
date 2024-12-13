using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Category.Requests.Queries.Validations
{
    public class GetCategoryFeaturesQueryValidation : AbstractValidator<GetCategoryFeaturesQueryRequest>
    {
        public GetCategoryFeaturesQueryValidation()
        {
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);
        }
    }
}
