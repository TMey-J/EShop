using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Product.Requests.Queries.Validations
{
    public class GetAllProductQueryValidation : AbstractValidator<GetAllProductsQueryRequest>
    {
        public GetAllProductQueryValidation()
        {
            RuleFor(x => x.Search.Title).
                MaximumLength(300).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.Search.EnglishTitle).
                MaximumLength(300).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.Search.EnglishTitle).
                MaximumLength(300).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.Search.SortingBy).IsInEnum().
            WithMessage(Messages.Validations.NotInEnum(0,1));
            
            RuleFor(x => x.Search.DeleteStatus).IsInEnum().
                WithMessage(Messages.Validations.NotInEnum(0,1));
            
            RuleFor(x => x.Search.SortingAs).IsInEnum().
                WithMessage(Messages.Validations.NotInEnum(0,1));
            
            RuleFor(x => x.Search.Pagination.TakeRecord).
                InclusiveBetween(1, 100).WithMessage(Messages.Validations.Between);
            
            RuleFor(x => x.Search.Pagination.CurrentPage).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);

        }
    }
}
