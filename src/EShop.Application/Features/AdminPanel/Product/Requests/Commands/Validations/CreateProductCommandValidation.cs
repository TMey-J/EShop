using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Product.Requests.Commands.Validations
{
    public class CreateProductCommandValidation : AbstractValidator<CreateProductCommandRequest>
    {
        public CreateProductCommandValidation()
        {
            RuleFor(x => x.Title).NotEmpty()
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(300).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.EnglishTitle).NotEmpty()
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(300).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.BasePrice).GreaterThan((uint)0)
                .WithMessage(Messages.Validations.GreaterThanZero);
            
            RuleFor(x => x.Count).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);

            RuleFor(x => x.Description).NotEmpty()
                .WithMessage(Messages.Validations.Required);

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween((byte)0, (byte)100)
                .WithMessage(Messages.Validations.Between);
            
            RuleFor(x => x.CategoryId).GreaterThan(0)
                .WithMessage(Messages.Validations.Required);
            
            RuleFor(x => x.ColorsCode).NotEmpty()
                .WithMessage(Messages.Validations.Required);
            
            RuleFor(x => x.Tags).NotEmpty()
                .WithMessage(Messages.Validations.Required);
            
            RuleFor(x => x.Images).NotEmpty()
                .WithMessage(Messages.Validations.Required);
        }
    }
}
