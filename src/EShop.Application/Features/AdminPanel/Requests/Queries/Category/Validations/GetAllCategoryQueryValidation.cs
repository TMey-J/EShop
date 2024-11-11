using EShop.Application.Features.AdminPanel.Requests.Commands.Category;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Requests.Queries.Category.Validations
{
    public class GetAllCategoryQueryValidation : AbstractValidator<GetAllCategoryQueryRequest>
    {
        public GetAllCategoryQueryValidation()
        {
            RuleFor(x => x.Search.Title).
                MaximumLength(100).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.Search.SortingBy).IsInEnum().
            WithMessage(Messages.Validations.NotInEnum(0,1));
            
            RuleFor(x => x.Search.DeleteStatus).IsInEnum().
                WithMessage(Messages.Validations.NotInEnum(0,1));
            
            RuleFor(x => x.Search.SortingAs).IsInEnum().
                WithMessage(Messages.Validations.NotInEnum(0,1));

            RuleFor(x => x.Search.Pagination.TakeRecord).
                InclusiveBetween(1, 100).WithMessage(Messages.Validations.Between);

        }
    }
}
