using EShop.Application.Features.AdminPanel.Tag.Requests.Commands;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Feature.Requests.Commands.Validations
{
    public class UpdateFeatureCommandValidation : AbstractValidator<UpdateFeatureCommandRequest>
    {
        public UpdateFeatureCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);
            RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.Validations.Required)
                .MaximumLength(100).WithMessage(Messages.Validations.MaxLength);
        }
    }
}
