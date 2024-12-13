using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Category.Requests.Queries.Validations
{
    public class GetAllCategoryQueryValidation : AbstractValidator<GetAllCategoryQueryRequest>
    {
        public GetAllCategoryQueryValidation()
        {
            RuleFor(x => x.Search.Title).MaximumLength(100)
                .When(s => s.Search != null)
                .WithMessage(Messages.Validations.MaxLength);

            RuleFor(x => x.Search.SortingBy).IsInEnum().When(s => s.Search != null)
                .WithMessage(Messages.Validations.NotInEnum(0, 1));

            RuleFor(x => x.Search.DeleteStatus).IsInEnum().When(s => s.Search != null)
                .WithMessage(Messages.Validations.NotInEnum(0, 1));

            RuleFor(x => x.Search.SortingAs).IsInEnum().When(s => s.Search != null)
                .WithMessage(Messages.Validations.NotInEnum(0, 1));

            RuleFor(x => x.Search.Pagination.TakeRecord).InclusiveBetween(1, 100).When(s => s.Search != null)
                .WithMessage(Messages.Validations.Between);

            RuleFor(x => x.Search.Pagination.CurrentPage).GreaterThan(0).When(s => s.Search != null)
                .WithMessage(Messages.Validations.GreaterThanZero);
        }
    }
}