using EShop.Application.Contracts.MongoDb;
using EShop.Application.Features.AdminPanel.Seller.Requests.Queries;
using EShop.Application.Features.AdminPanel.User.Requests.Queries;

namespace EShop.Application.Features.AdminPanel.Seller.Handlers.Queries;

public class GetSellerQueryHandler(IMongoSellerRepository seller) :
    IRequestHandler<GetSellerQueryRequest, GetSellerQueryResponse>
{
    private readonly IMongoSellerRepository _seller = seller;

    public async Task<GetSellerQueryResponse> Handle(GetSellerQueryRequest request, CancellationToken cancellationToken)
    {
        var seller = await _seller.FindByIdAsync(request.Id) ??
                   throw new NotFoundException(NameToReplaceInException.Seller);
        
        var showSellerDetails = new ShowSellerDetailsDto(
            seller.RejectReason,
            seller.PostalCode,
            seller.Address,
            seller.IndividualSeller?.NationalId,
            seller.IndividualSeller?.CartOrShebaNumber,
            seller.IndividualSeller?.AboutSeller,
            seller.LegalSeller?.CompanyName,
            seller.LegalSeller?.RegisterNumber,
            seller.LegalSeller?.EconomicCode,
            seller.LegalSeller?.SignatureOwners,
            seller.LegalSeller?.CompanyType
            );
        
        var showSeller = new ShowSellerDto(seller.Id,
            seller.UserId,
            seller.IsLegalPerson,
            seller.ShopName,
            seller.Logo,
            seller.Website,
            seller.City?.Title??"",
            seller.City?.Province?.Title??"",
            seller.CreatedDateTime,
            seller.DocumentStatus,
            showSellerDetails);
        return new GetSellerQueryResponse(showSeller);
    }
}