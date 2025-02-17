using FluentValidation;

namespace EShop.Application.Features.SellerPanel.Requests.Queries.Validations;

public class GetAllReservedProductsQueryValidation: AbstractValidator<GetAllReservedProductsQueryRequest>
{
    public GetAllReservedProductsQueryValidation()
    {
        RuleFor(x => x.Search.Title).MaximumLength(300)
            .WithMessage(Messages.Validations.MaxLength);
        RuleFor(x => x.SellerId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.Search.SortingBy).IsInEnum()
            .WithMessage(Messages.Validations.NotInEnum(0, 1));

        RuleFor(x => x.Search.DeleteStatus).IsInEnum()
            .WithMessage(Messages.Validations.NotInEnum(0, 1));

        RuleFor(x => x.Search.SortingAs).IsInEnum()
            .WithMessage(Messages.Validations.NotInEnum(0, 1));

        RuleFor(x => x.Search.Pagination.TakeRecord).InclusiveBetween(1, 100)
            .WithMessage(Messages.Validations.Between);

        RuleFor(x => x.Search.Pagination.CurrentPage).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
