using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Category.Requests.Queries.Validations
{
    public class GetCategoryCommandValidation : AbstractValidator<GetCategoryQueryRequest>
    {
        public GetCategoryCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);
        }
    }
}
