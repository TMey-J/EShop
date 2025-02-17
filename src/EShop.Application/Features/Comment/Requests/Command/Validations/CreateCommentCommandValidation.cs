using FluentValidation;

namespace EShop.Application.Features.Comment.Requests.Command.Validations;

public class CreateCommentCommandValidation : AbstractValidator<CreateCommentCommandRequest>
{
    public CreateCommentCommandValidation()
    {
        RuleFor(x => x.ProductId).GreaterThan(0)
            .WithMessage(Messages.Validations.GreaterThanZero);
        RuleFor(x => x.Body).NotEmpty()
            .WithMessage(Messages.Validations.Required);
        RuleFor(x => x.Rating)
            .InclusiveBetween((byte)1, (byte)5)
            .WithMessage(Messages.Validations.Between);
    }
}