using Blog.Core.Application.Constants.Common;
using EShop.Application.Constants.Common;
using FluentValidation;

namespace EShop.Application.Features.Authorize.Requests.Commands.Validations;

public class VerifyEmailCommandValidation: AbstractValidator<VerifyEmailCommandRequest>
{
    public VerifyEmailCommandValidation()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage(Messages.Validations.Required);

        RuleFor(x => x.Email).NotEmpty().WithMessage(Messages.Validations.Required)
           .MaximumLength(256).WithMessage(Messages.Validations.MaxLength)
           .Matches(RegularExperssions.Email).WithMessage(Messages.Validations.RegularExpression);
    }
}
