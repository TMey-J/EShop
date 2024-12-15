using EShop.Application.Features.AdminPanel.User.Requests.Queries;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Seller.Requests.Queries.Validations
{
    public class GetAllSellersQueryValidation : AbstractValidator<GetAllSellersQueryRequest>
    {
        public GetAllSellersQueryValidation()
        {
            RuleFor(x => x.Search.UserName).
                MaximumLength(256).WithMessage(Messages.Validations.MaxLength);
            RuleFor(x => x.Search.ShopName).
                MaximumLength(200).WithMessage(Messages.Validations.MaxLength);
            RuleFor(x => x.Search.Province).
                MaximumLength(200).WithMessage(Messages.Validations.MaxLength);
            RuleFor(x => x.Search.City).
                MaximumLength(200).WithMessage(Messages.Validations.MaxLength);
            
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
