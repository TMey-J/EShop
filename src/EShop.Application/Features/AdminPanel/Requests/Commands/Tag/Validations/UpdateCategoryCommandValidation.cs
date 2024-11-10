using EShop.Application.Features.AdminPanel.Requests.Commands.Category;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Requests.Commands.Tag.Validations
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
