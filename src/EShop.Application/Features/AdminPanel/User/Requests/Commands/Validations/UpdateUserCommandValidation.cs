using EShop.Application.Features.Authorize.Requests.Commands;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.User.Requests.Commands.Validations;

public class UpdateUserCommandValidation: AbstractValidator<UpdateUserCommandRequest>
{
    public UpdateUserCommandValidation()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage(Messages.Validations.Required)
            .MaximumLength(256).WithMessage(Messages.Validations.MaxLength);

        RuleFor(x => x.EmailOrPhoneNumber).NotEmpty().WithMessage(Messages.Validations.Required)
            .MaximumLength(256).WithMessage(Messages.Validations.MaxLength)
            .Matches(RegularExperssions.EmailOrPhoneNumber).WithMessage(Messages.Validations.RegularExpression);

        RuleFor(x => x.Password).Length(8, 64).WithMessage(Messages.Validations.Length);
        
        RuleFor(x => x.Roles).NotEmpty().WithMessage(Messages.Validations.Required);
    }
}
