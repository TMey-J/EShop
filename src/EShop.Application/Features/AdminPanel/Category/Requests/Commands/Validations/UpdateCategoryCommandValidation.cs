﻿using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Category.Requests.Commands.Validations
{
    public class UpdateCategoryCommandValidation : AbstractValidator<UpdateCategoryCommandRequest>
    {
        public UpdateCategoryCommandValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(Messages.Validations.Required)
                .MaximumLength(100).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.ParentId)
                .GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);
            
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(Messages.Validations.GreaterThanZero);
        }
    }
}
