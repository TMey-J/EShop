using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Tag.Requests.Queries.Validations
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
