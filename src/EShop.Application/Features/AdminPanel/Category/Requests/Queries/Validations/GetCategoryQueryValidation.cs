using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Category.Requests.Queries.Validations
{
    public class GetCategoryQueryValidation : AbstractValidator<GetCategoryQueryRequest>
    {
        public GetCategoryQueryValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);
        }
    }
}
