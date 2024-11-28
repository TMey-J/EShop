using FluentValidation;

namespace EShop.Application.Features.AdminPanel.User.Requests.Queries.Validations
{
    public class GetAllUsersQueryValidation : AbstractValidator<GetAllUsersQueryRequest>
    {
        public GetAllUsersQueryValidation()
        {
            RuleFor(x => x.Search.UserName).
                MaximumLength(256).WithMessage(Messages.Validations.MaxLength);
            RuleFor(x => x.Search.Email).
                MaximumLength(256).WithMessage(Messages.Validations.MaxLength);
            RuleFor(x => x.Search.PhoneNumber).
                MaximumLength(13).WithMessage(Messages.Validations.MaxLength);
            
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
