using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Tag.Requests.Queries.Validations
{
    public class GetAllTagsQueryValidation : AbstractValidator<GetAllTagsQueryRequest>
    {
        public GetAllTagsQueryValidation()
        {
            // RuleFor(x => x.Search.Title).
            //     MaximumLength(100).WithMessage(Messages.Validations.MaxLength);
            //
            // RuleFor(x => x.Search.SortingBy).IsInEnum().
            // WithMessage(Messages.Validations.NotInEnum(0,1));
            //
            // RuleFor(x => x.Search.DeleteStatus).IsInEnum().
            //     WithMessage(Messages.Validations.NotInEnum(0,1));
            //
            // RuleFor(x => x.Search.SortingAs).IsInEnum().
            //     WithMessage(Messages.Validations.NotInEnum(0,1));
            //
            // RuleFor(x => x.Search.Pagination.TakeRecord).
            //     InclusiveBetween(1, 100).WithMessage(Messages.Validations.Between);
            //
            // RuleFor(x => x.Search.Pagination.CurrentPage).GreaterThan(0)
            //     .WithMessage(Messages.Validations.GreaterThanZero);

        }
    }
}
