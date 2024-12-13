using FluentValidation;

namespace EShop.Application.Features.AdminPanel.User.Requests.Queries.Validations
{
    public class GetUserQueryValidation : AbstractValidator<GetUserQueryRequest>
    {
        public GetUserQueryValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);

        }
    }
}
