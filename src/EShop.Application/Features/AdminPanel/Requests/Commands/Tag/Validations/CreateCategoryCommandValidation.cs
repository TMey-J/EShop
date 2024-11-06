using EShop.Application.Features.AdminPanel.Requests.Commands.Category;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Requests.Commands.Tag.Validations
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
