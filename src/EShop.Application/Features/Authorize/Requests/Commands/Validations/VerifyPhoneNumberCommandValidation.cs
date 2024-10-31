using Blog.Core.Application.Constants.Common;
using EShop.Application.Constants.Common;
using FluentValidation;

namespace EShop.Application.Features.Authorize.Requests.Commands.Validations;

public class VerifyPhoneNumberCommandValidation: AbstractValidator<VerifyPhoneNumberCommandRequest>
{
    public VerifyPhoneNumberCommandValidation()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage(Messages.Validations.Required);

        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(Messages.Validations.Required)
           .MaximumLength(13).WithMessage(Messages.Validations.MaxLength)
           .Matches(RegularExperssions.PhoneNumber).WithMessage(Messages.Validations.RegularExpression);
    }
}
