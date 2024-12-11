using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Category.Requests.Commands.Validations
{
    public class AddFeaturesToCategoryCommandValidation : AbstractValidator<AddFeaturesToCategoryCommandRequest>
    {
        public AddFeaturesToCategoryCommandValidation()
        {
            RuleFor(x => x.CategoryId).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);
            RuleFor(x => x.FeaturesName).NotEmpty()
                .WithMessage(Messages.Validations.Required);
            RuleFor(x => x.FeaturesName).ForEach(f =>
            {
                f.NotEmpty().WithMessage(Messages.Validations.Required);
            });
        }
    }
}
