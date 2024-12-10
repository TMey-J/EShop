using EShop.Application.Features.AdminPanel.Category.Requests.Commands;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Feature.Requests.Commands.Validations;

public class CreateFeatureCommandValidation:AbstractValidator<CreateFeatureCommandRequest>
{
    public CreateFeatureCommandValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.Validations.Required);
    }
}