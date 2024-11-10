using Blog.Core.Application.Constants.Common;
using EShop.Application.Constants.Common;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Requests.Commands.Category.Validations
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
