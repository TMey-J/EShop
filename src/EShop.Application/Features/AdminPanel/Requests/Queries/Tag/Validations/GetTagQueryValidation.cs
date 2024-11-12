using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Requests.Queries.Tag.Validations
{
    public class GetTagQueryValidation : AbstractValidator<GetTagQueryRequest>
    {
        public GetTagQueryValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);

        }
    }
}
