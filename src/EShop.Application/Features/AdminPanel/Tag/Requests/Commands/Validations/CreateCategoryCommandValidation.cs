using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Tag.Requests.Commands.Validations
{
    public class CreateTagCommandValidation : AbstractValidator<CreateTagCommandRequest>
    {
        public CreateTagCommandValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(Messages.Validations.Required)
                .MaximumLength(100).WithMessage(Messages.Validations.MaxLength);
        }
    }
}
