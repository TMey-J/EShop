using FluentValidation;

namespace EShop.Application.Features.Order.Requests.Queries.Validations;

public class GetAllOrdersQueryValidation: AbstractValidator<GetAllOrdersQueryRequest>
{
    public GetAllOrdersQueryValidation()
    {
        RuleFor(x => x.UserId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
    }
}
