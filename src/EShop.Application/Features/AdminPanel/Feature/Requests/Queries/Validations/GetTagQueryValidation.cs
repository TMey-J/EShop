using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Feature.Requests.Queries.Validations
{
    public class GetFeatureQueryValidation : AbstractValidator<GetFeatureQueryRequest>
    {
        public GetFeatureQueryValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);

        }
    }
}
