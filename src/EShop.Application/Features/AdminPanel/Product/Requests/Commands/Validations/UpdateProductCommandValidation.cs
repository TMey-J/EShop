using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Product.Requests.Commands.Validations
{
    public class UpdateProductCommandValidation : AbstractValidator<UpdateProductCommandRequest>
    {
        public UpdateProductCommandValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);
            RuleFor(x => x.Title).NotEmpty()
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(300).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.EnglishTitle).NotEmpty()
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(300).WithMessage(Messages.Validations.MaxLength);

            RuleFor(x => x.Description).NotEmpty()
                .WithMessage(Messages.Validations.Required);
            
            RuleFor(x => x.CategoryId).GreaterThan(0)
                .WithMessage(Messages.Validations.Required);
            
            RuleFor(x => x.Tags).NotEmpty()
                .WithMessage(Messages.Validations.Required);
        }
    }
}
