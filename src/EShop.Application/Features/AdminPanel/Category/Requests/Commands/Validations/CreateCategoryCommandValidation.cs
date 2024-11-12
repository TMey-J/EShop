using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Category.Requests.Commands.Validations
{
    public class CreateCategoryCommandValidation : AbstractValidator<CreateCategoryCommandRequest>
    {
        public CreateCategoryCommandValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(Messages.Validations.Required)
                .MaximumLength(100).WithMessage(Messages.Validations.MaxLength);
        }
    }
}
