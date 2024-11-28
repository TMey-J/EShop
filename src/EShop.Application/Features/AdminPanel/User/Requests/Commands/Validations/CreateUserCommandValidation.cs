using EShop.Application.Features.Authorize.Requests.Commands;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.User.Requests.Commands.Validations;

public class CreateUserCommandValidation: AbstractValidator<CreateUserCommandRequest>
{
    public CreateUserCommandValidation()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage(Messages.Validations.Required)
            .MaximumLength(256).WithMessage(Messages.Validations.MaxLength);

        RuleFor(x => x.EmailOrPhoneNumber).NotEmpty().WithMessage(Messages.Validations.Required)
           .MaximumLength(256).WithMessage(Messages.Validations.MaxLength)
           .Matches(RegularExperssions.EmailOrPhoneNumber).WithMessage(Messages.Validations.RegularExpression);

        RuleFor(x => x.Password).NotEmpty().WithMessage(Messages.Validations.Required)
            .Length(8, 64).WithMessage(Messages.Validations.Length)
            .Equal(x => x.ConfirmPassword).WithMessage("رمز عبور با تکرار آن برابر نیست");
        
        RuleFor(x => x.Roles).NotEmpty().WithMessage(Messages.Validations.Required);
    }
}
