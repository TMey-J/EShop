using EShop.Application.Features.Comment.Requests.Command;
using FluentValidation;

namespace EShop.Application.Features.Comment.Requests.Queries.Validations;

public class GetAllCommentsQueryValidation : AbstractValidator<GetAllCommentsQueryRequest>
{
    public GetAllCommentsQueryValidation()
    {
        RuleFor(x => x.ProductId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.Pagination.TakeRecord).InclusiveBetween(1,100)
            .WithMessage(Messages.Validations.Between);
        RuleFor(x => x.Pagination.CurrentPage).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}