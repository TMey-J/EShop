using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Tag.Requests.Commands.Validations
{
    public class UpdateTagCommandValidation : AbstractValidator<UpdateTagCommandRequest>
    {
        public UpdateTagCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);
            RuleFor(x => x.Title).NotEmpty().WithMessage(Messages.Validations.Required)
                .MaximumLength(100).WithMessage(Messages.Validations.MaxLength);
        }
    }
}
