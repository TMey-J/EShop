using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Seller.Handlers.Queries;

public class GetSellerQueryHandler(IMongoSellerRepository seller) :
    IRequestHandler<GetSellerQueryRequest, GetSellerQueryResponse>
{
    private readonly IMongoSellerRepository _seller = seller;

    public async Task<GetSellerQueryResponse> Handle(GetSellerQueryRequest request, CancellationToken cancellationToken)
    {
        var seller = await _seller.FindByIdAsync(request.Id) ??
                   throw new NotFoundException(NameToReplaceInException.Seller);
        LegalSellerDto? legalSeller = null;
        IndividualSellerDto? individualSeller = null;
        if (seller.LegalSeller!=null)
        {
            legalSeller = new LegalSellerDto
            {
                CompanyName = seller.LegalSeller.CompanyName,
                RegisterNumber = seller.LegalSeller.RegisterNumber,
                EconomicCode = seller.LegalSeller.EconomicCode,
                SignatureOwners = seller.LegalSeller.SignatureOwners,
                ShabaNumber = seller.LegalSeller.ShabaNumber,
                CompanyType = seller.LegalSeller.CompanyType
            };
        }
        if(seller.IndividualSeller!=null)
        {
            individualSeller = new IndividualSellerDto
            {
                NationalId = seller.IndividualSeller.NationalId,
                CartOrShebaNumber = seller.IndividualSeller.CartOrShebaNumber,
                AboutSeller = seller.IndividualSeller.AboutSeller
            };
        }
        var showSeller = new ShowSellerDto(seller.Id,
            seller.UserId,
            seller.UserName,
            seller.IsLegalPerson,
            seller.ShopName,
            seller.Logo,
            seller.Website,
            seller.City?.Title??"",
            seller.City?.Province?.Title??"",
            seller.PostalCode,
            seller.Address,
            seller.RejectReason,
            seller.CreatedDateTime,
            seller.DocumentStatus,
            legalSeller,
            individualSeller
            );
        return new GetSellerQueryResponse(showSeller);
    }
}