using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;
using FluentValidation;

namespace EShop.Application.Features.AdminPanel.Seller.Requests.Commands.Validations
{
    public class UpdateSellersQueryValidation : AbstractValidator<UpdateSellerCommandRequest>
    {
        public UpdateSellersQueryValidation()
        {
            RuleFor(x => x.SellerId).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);
            
            RuleFor(x => x.ShopName).NotEmpty()
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(200).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.Website).NotEmpty()
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(200).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.CityId).GreaterThan(0)
                .WithMessage(Messages.Validations.GreaterThanZero);
            
            RuleFor(x => x.PostalCode).NotEmpty()
                .WithMessage(Messages.Validations.Required)
                .Length(10).WithMessage(Messages.Validations.ExactLength);
            
            RuleFor(x => x.Address).NotEmpty()
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(300).WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.IndividualSeller!.NationalId).NotEmpty()
                .When(x=>x.IndividualSeller!=null)
                .WithMessage(Messages.Validations.Required)
                .Length(10)
                .When(x => x.IndividualSeller!=null)
                .WithMessage(Messages.Validations.ExactLength);
            
            RuleFor(x => x.IndividualSeller!.CartOrShebaNumber).NotEmpty()
                .When(x=>x.IndividualSeller!=null)
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(24)
                .When(x => x.IndividualSeller!=null)
                .WithMessage(Messages.Validations.MaxLength);

            RuleFor(x => x.IndividualSeller!.AboutSeller).NotEmpty()
                .When(x => x.IndividualSeller!=null)
                .WithMessage(Messages.Validations.Required);
            
            RuleFor(x => x.LegalSeller!.CompanyName).NotEmpty()
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(200)
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.LegalSeller!.RegisterNumber).NotEmpty()
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(100)
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.LegalSeller!.EconomicCode)
                .Length(12)
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.ExactLength);
            
            RuleFor(x => x.LegalSeller!.SignatureOwners).NotEmpty()
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.Required)
                .MaximumLength(300)
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.MaxLength);
            
            RuleFor(x => x.LegalSeller!.ShabaNumber).NotEmpty()
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.Required)
                .Length(24)
                .When(x=>x.LegalSeller!=null)
                .WithMessage(Messages.Validations.MaxLength);

            RuleFor(x => x.LegalSeller!.CompanyType).IsInEnum()
                .When(x => x.LegalSeller!=null)
                .WithMessage(Messages.Validations.NotInEnum(0, 4));

        }
    }
}
