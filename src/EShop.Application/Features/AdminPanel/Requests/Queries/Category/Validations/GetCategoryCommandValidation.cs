using EShop.Application.Features.AdminPanel.Requests.Commands.Category;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Requests.Queries.Category.Validations
{
    public class GetCategoryCommandValidation : AbstractValidator<GetCategoryQueryRequest>
    {
        public GetCategoryCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);
        }
    }
}
